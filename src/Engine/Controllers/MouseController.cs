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
            InitPrefab();
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

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
        private List<GameObject> _dragPreviewGameObjects = new List<GameObject>();

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

        private void InitPrefab()
        {
            var spritesheet = new SpriteSheet();
            spritesheet.Load(Engine.Instance.Path("assets", "base", "cursors", "cursors.xml"));

            _circleCursorPrefab = new GameObject();
            _circleCursorPrefab.Name = "Cursor";
            _circleCursorPrefab.Position = new Vector2<float>();

            _circleCursorPrefab.SpriteSheet = spritesheet;
            _circleCursorPrefab.Sprite = spritesheet._sprites["cursor"];
            _circleCursorPrefab.Sprite.Centered = true;
            _circleCursorPrefab.SpriteSheet.SortingLayer = "Cursor";
        }

        public void Update()
        {
            _currentFramePosition = CameraController.Instance.ScreenToWorldPoint(SDLEvent.MousePosition);

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

            // Start Drag
            if (SDLEvent.MouseButtonWentDown(SDL.SDL_BUTTON_LEFT))
            {
                _dragStartPosition = _currentFramePosition;
            }

            var startX = Mathf.FloorToInt(_dragStartPosition.X);
            var endX = Mathf.FloorToInt(_currentFramePosition.X);
            var startY = Mathf.FloorToInt(_dragStartPosition.Y);
            var endY = Mathf.FloorToInt(_currentFramePosition.Y);
            
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

            // Clear the drag-area markers
            while (_dragPreviewGameObjects.Count > 0)
            {
                var go = _dragPreviewGameObjects[0];
                _dragPreviewGameObjects.RemoveAt(0);
                SimplePool.Despawn(go);
            }

            // Put one marker at the mouse position for testing
            //var cursor = SimplePool.Spawn(_circleCursorPrefab, new Vector2<float>(_currentFramePosition.X, _currentFramePosition.Y), 0);
            //cursor.Sprite.Centered = true;
            //cursor.Name = "mousepos";
            //_dragPreviewGameObjects.Add(cursor);

            if (SDLEvent.MouseButtonIsDown(SDL.SDL_BUTTON_LEFT))
            {
                // Display dragged area
                for (var x = startX; x <= endX; x++)
                {
                    for (var y = startY; y <= endY; y++)
                    {
                        var t = World.Instance.GetTileAt(x, y);
                        if (t != null)
                        {
                            var go = SimplePool.Spawn(_circleCursorPrefab, new Vector2<float>(x + 0.5f, y + 0.5f), 0);
                            //go.transform.SetParent(this.transform, true);
                            _dragPreviewGameObjects.Add(go);
                        }
                    }
                }
            }

            // End Drag
            if (SDLEvent.MouseButtonWentUp(SDL.SDL_BUTTON_LEFT))
            {
                //var bmc = GameObject.FindObjectOfType<BuildModeController>();

                for (var x = startX; x <= endX; x++)
                {
                    for (var y = startY; y <= endY; y++)
                    {
                        //var t = WorldController.Instance.World.GetTileAt(x, y);
                        //if (t != null)
                        //{
                        //    bmc.DoBuild(t);
                        //}
                    }
                }
            }
        }
    }
}
