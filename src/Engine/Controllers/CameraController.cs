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
            Position = new Vector3();
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public Vector3 Position { get; set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
        public void Update()
        {
            // Check for movement input
            float scrollSpeed = 50f;

            if (SDLEvent.KeyState(SDL_Keycode.SDLK_a))
            {
                Position += Vector3.Left * Time.DeltaTime * scrollSpeed;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_d))
            {
                Position += Vector3.Right * Time.DeltaTime * scrollSpeed;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_w))
            {
                Position += Vector3.Up * Time.DeltaTime * scrollSpeed;
            }
            if (SDLEvent.KeyState(SDL_Keycode.SDLK_s))
            {
                Position += Vector3.Down * Time.DeltaTime * scrollSpeed;
            }
        }
    }
}
