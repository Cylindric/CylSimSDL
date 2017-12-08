using Engine.Models;
using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using SDL2;
using System;
using System.Collections.Generic;

namespace Engine.Controllers
{
    internal class MouseController
    {
        #region Singleton
        private static readonly Lazy<MouseController> _instance = new Lazy<MouseController>(() => new MouseController());

        public static MouseController Instance { get { return _instance.Value; } }

        private MouseController()
        {
            Start();
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        private enum MouseMode
        {
            Select,
            Build
        }

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */

        /// <summary>
        /// The world-coordinates of the mouse cursor on the last frame.
        /// </summary>
        private Vector2<float> _lastFramePosition;

        /// <summary>
        /// The current world-coordinates of the mouse cursor.
        /// </summary>
        private Vector2<float> _currentFramePosition;

        /// <summary>
        /// The current screen-coordinates of the mouse cursor.
        /// </summary>
        // private Vector2<int> _currentScreenPosition;

        private Vector2<float> _dragStartPosition;
        private GameObject _circleCursorPrefab;
        private List<GameObject> _dragPreviewGameObjects;
        private bool _isDragging = false;
        private MouseMode _mode = MouseMode.Build;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        private void Start()
        {
            _dragPreviewGameObjects = new List<GameObject>();

            var spritesheet = new SpriteSheet();
            spritesheet.Load(Engine.Instance.Path("assets", "base", "cursors", "cursors.xml"));

            _circleCursorPrefab = new GameObject();
            _circleCursorPrefab.Name = "Cursor";
            _circleCursorPrefab.Position = new Vector2<float>();

            _circleCursorPrefab.SpriteSheet = spritesheet;
            _circleCursorPrefab.Sprite = spritesheet.Sprites["cursor"];
            _circleCursorPrefab.Sprite.Centered = true;
            _circleCursorPrefab.SpriteSheet.SortingLayer = "Cursor";
        }

        public void StartBuildMode()
        {
            _mode = MouseMode.Build;
        }

        public void StartSelectMode()
        {
            _mode = MouseMode.Select;
        }

        /// <summary>
        /// Gets the current mouse position, in World-space coordinates.
        /// </summary>
        /// <returns></returns>
        public Vector2<float> GetMousePosition()
        {
            return _currentFramePosition;
        }

        public Tile GetTileUnderMouse()
        {
            return WorldController.Instance.GetTileAtWorldCoordinates(_currentFramePosition);
        }

        public void Update()
        {
            _currentFramePosition = CameraController.Instance.ScreenToWorldPoint(SDLEvent.MousePosition);

            if (SDLEvent.KeyUp(SDL.SDL_Keycode.SDLK_ESCAPE))
            {
                if (_mode == MouseMode.Build)
                {
                    _mode = MouseMode.Select;
                }
                else if (_mode == MouseMode.Select)
                {
                    // TODO: Show game menu
                }
            }

            UpdateDragging();
            UpdateCameraMovement();

            _lastFramePosition = CameraController.Instance.ScreenToWorldPoint(SDLEvent.MousePosition);
        }

        public void Render()
        {
            foreach(var go in _dragPreviewGameObjects)
            {
                var screenPosition = CameraController.Instance.WorldToScreenPoint(go.Position);
                go.SpriteSheet.Render(go.Sprite, screenPosition);
            }
        }

        private void UpdateCameraMovement()
        {
            // Handle screen dragging
            if (SDLEvent.MouseButtonIsDown(SDL2.SDL.SDL_BUTTON_MIDDLE))
            {
                var diff = new Vector2<int>();
                diff.X = (int)(_lastFramePosition.X - _currentFramePosition.X);
                diff.Y = (int)(_lastFramePosition.Y - _currentFramePosition.Y);

                var pos = new Vector2<int>();
                pos.X = CameraController.Instance.Position.X + diff.X;
                pos.Y = CameraController.Instance.Position.Y - diff.Y;

                CameraController.Instance.SetPosition(pos);

                // Zooming
                // Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel") * 2f;
                // Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 50f);
            }
        }

        private void UpdateDragging()
        {
            /*
            // If over UI, do nothing
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            */

            // Clear the drag-area markers
            while (_dragPreviewGameObjects.Count > 0)
            {
                var go = _dragPreviewGameObjects[0];
                _dragPreviewGameObjects.RemoveAt(0);
                SimplePool.Despawn(go);
            }

            if (_mode != MouseMode.Build)
            {
                return;
            }

            // Start Drag
            if (SDLEvent.MouseButtonWentDown(SDL.SDL_BUTTON_LEFT))
            {
                _dragStartPosition = _currentFramePosition;
                _isDragging = true;
            }
            else if (_isDragging == false)
            {
                _dragStartPosition = _currentFramePosition;
            }
            if (SDLEvent.MouseButtonWentUp(SDL.SDL_BUTTON_RIGHT) || SDLEvent.KeyUp(SDL.SDL_Keycode.SDLK_ESCAPE))
            {
                // The RIGHT mouse button came up or ESC was pressed, so cancel any dragging.
                _isDragging = false;
            }

            //if (_bmc.IsObjectDraggable() == false)
            //{
            //    _dragStartPosition = _currentFramePosition;
            //}


            var startX = Mathf.FloorToInt(_dragStartPosition.X + 0.5f);
            var endX = Mathf.FloorToInt(_currentFramePosition.X + 0.5f);
            var startY = Mathf.FloorToInt(_dragStartPosition.Y + 0.5f);
            var endY = Mathf.FloorToInt(_currentFramePosition.Y + 0.5f);
            
            if (endX < startX)
            {
                var temp = endX;
                endX = startX;
                startX = temp;
            }

            if (endY < startY)
            {
                var temp = endY;
                endY = startY;
                startY = temp;
            }

            // Put one marker at the mouse position for testing
            //var cursor = SimplePool.Spawn(_circleCursorPrefab, new Vector2<float>(_currentFramePosition.X, _currentFramePosition.Y), 0);
            //cursor.Sprite.Centered = true;
            //cursor.Name = "mousepos";
            //_dragPreviewGameObjects.Add(cursor);

            // Display dragged area
            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    var t = World.Instance.GetTileAt(x, y);
                    if (t != null)
                    {
                        var actionTile = true;

                        // If shift is being held, just action the perimeter
                        if (SDLEvent.KeyState(SDL.SDL_Keycode.SDLK_LSHIFT) || SDLEvent.KeyState(SDL.SDL_Keycode.SDLK_RSHIFT))
                        {
                            actionTile = (x == startX || x == endX || y == startY || y == endY);
                        }

                        if (actionTile)
                        {
                            //if (_bmc.BuildMode == BuildMode.Furniture)
                            //{
                            //    ShowFurnitureSpriteAtTile(_bmc.BuildModeObjectType, t);
                            //}
                            //else
                            //{
                                var go = SimplePool.Spawn(_circleCursorPrefab, new Vector2<float>(x, y), 0);
                                // go.transform.SetParent(this.transform, true);
                                _dragPreviewGameObjects.Add(go);
                            //}
                        }
                    }
                }
            }

            // End Drag
            if (_isDragging && SDLEvent.MouseButtonWentUp(SDL.SDL_BUTTON_LEFT))
            {
                _isDragging = false;
                for (var x = startX; x <= endX; x++)
                {
                    for (var y = startY; y <= endY; y++)
                    {
                        var actionTile = true;

                        // If shift is being held, just action the perimeter
                        if (SDLEvent.KeyState(SDL.SDL_Keycode.SDLK_LSHIFT) || SDLEvent.KeyState(SDL.SDL_Keycode.SDLK_RSHIFT))
                        {
                            actionTile = (x == startX || x == endX || y == startY || y == endY);
                        }

                        if (actionTile)
                        {
                            var t = World.Instance.GetTileAt(x, y);
                            if (t != null)
                            {
                                // _bmc.DoBuild(t);
                            }
                        }
                    }
                }
            }
        }
    }
}
