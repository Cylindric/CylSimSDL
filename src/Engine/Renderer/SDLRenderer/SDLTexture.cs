using Engine.Models;
using Engine.Utilities;
using SDL2;
using System;
using System.Diagnostics;
using static SDL2.SDL;

namespace Engine.Renderer.SDLRenderer
{
    [DebuggerDisplay("SDLTexture [{ptr}]")]
    internal class SDLTexture
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        public IntPtr ptr;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */
        public SDLTexture()
        {
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
        public void Load(string filename)
        {
            var surface = SDL_image.IMG_Load(filename);
            if(surface == null)
            {
                Log.Instance.Debug($"Failed to load image! SDL error: {SDL.SDL_GetError()}");
                throw new InvalidOperationException(SDL.SDL_GetError());
            }

            ptr = SDL.SDL_CreateTextureFromSurface(SDLRenderer.Instance.ptr, surface);
            SDL.SDL_DestroyTexture(surface);

            if(ptr == null)
            {
                Log.Instance.Debug($"Failed to create texture! SDL error: {SDL.SDL_GetError()}");
                throw new InvalidOperationException(SDL.SDL_GetError());
            }
        }

        public void RenderSprite(Sprite sprite, int x, int y, int width, int height)
        {
            SDL_Rect srcrect = new SDL_Rect()
            {
                x = sprite.X,
                y = sprite.Y,
                w = sprite.Width,
                h = sprite.Height
            };

            SDL_Rect dsrect = new SDL_Rect()
            {
                x = x,
                y = y,
                w = width,
                h = height
            };

            if(SDL.SDL_RenderCopy(SDLRenderer.Instance.ptr, ptr, ref srcrect, ref dsrect) < 0)
            {
                Log.Instance.Debug($"Failed to render texture! SDL error: {SDL.SDL_GetError()}");
                throw new InvalidOperationException(SDL.SDL_GetError());
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                SDL.SDL_DestroyTexture(ptr);
                disposedValue = true;
            }
        }

        ~SDLTexture()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
