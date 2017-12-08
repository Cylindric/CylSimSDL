using Engine.Models;
using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using System;
using static SDL2.SDL;

namespace Engine.Controllers
{
    public class CameraController
    {
        #region Singleton
        private static readonly Lazy<CameraController> _instance = new Lazy<CameraController>(() => new CameraController());

        public static CameraController Instance { get { return _instance.Value; } }

        private CameraController()
        {
            _text.Create("Testing 1..2..3..", "Robotica", 20, new SDL_Color() { r = 1, g = 1, b = 0, a = 0 });
            _text.Position = new Vector2<int>(5, 5);
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */

        /// <summary>
        /// The position of the camera. Stored internally as a float to allow for
        /// scrolling less than a single pixel in one frame.
        /// </summary>
        private Vector2<float> _position = new Vector2<float>();

        private Text _text = new Text();

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /// <summary>
        /// The current position of the camera.
        /// </summary>
        public Vector2<int> Position {
            get {
                return new Vector2<int>((int)_position.X, (int)_position.Y);
            }
            set
            {
                _position.X = value.X;
                _position.Y = value.Y;
            }
        }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        public void SetPosition(int x, int y)
        {
            _position = new Vector2<float>(x, y);
        }

        public void SetPosition(Vector2<int> pos)
        {
            _position = new Vector2<float>(pos.X, pos.Y);
        }

        public void Update()
        {
            // Check for movement input
            float scrollSpeed = 100f;
            var dragDistance = Time.DeltaTime * scrollSpeed;

            if (SDLEvent.KeyState(SDL_Keycode.SDLK_a))
            {
                _position.X -= dragDistance;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_d))
            {
                _position.X += dragDistance;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_w))
            {
                _position.Y += dragDistance;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_s))
            {
                _position.Y -= dragDistance;
            }

        }

        public void Render()
        {
            _text.Render();
        }

        internal Vector2<float> ScreenToWorldPoint(Vector2<int> screenPosition)
        {
            return new Vector2<float>()
            {
                X = (float)(screenPosition.X + Position.X) / TileSpriteController.GRID_SIZE,
                Y = (float)(screenPosition.Y - Position.Y) / TileSpriteController.GRID_SIZE
            };
        }

        internal Vector2<int> WorldToScreenPoint(Vector2<float> worldCoordinate)
        {
            return new Vector2<int>()
            {
                X = (int)(worldCoordinate.X * TileSpriteController.GRID_SIZE - Position.X),
                Y = (int)(worldCoordinate.Y * TileSpriteController.GRID_SIZE + Position.Y)
            };
        }
    }
}
