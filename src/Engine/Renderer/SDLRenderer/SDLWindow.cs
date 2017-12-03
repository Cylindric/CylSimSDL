using Engine.Utilities;
using SDL2;
using System;
using System.Diagnostics;

namespace Engine.Renderer.SDLRenderer
{
    [DebuggerDisplay("SDLWindow [{ptr}]")]
    internal class SDLWindow
    {
        #region Singleton
        private static readonly Lazy<SDLWindow> _instance = new Lazy<SDLWindow>(() => new SDLWindow());

        public static SDLWindow Instance { get { return _instance.Value; } }

        private SDLWindow()
        {
        }
        #endregion

        private const int SCREEN_X = 100;
        private const int SCREEN_Y = 100;
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 600;

        public IntPtr ptr;

        public void Start()
        {
            if(SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING)  < 0)
            {
                Log.Instance.Debug($"Failed to initialise SDL! SDL error: {SDL.SDL_GetError()}");
                throw new InvalidOperationException(SDL.SDL_GetError());
            }
            ptr = SDL.SDL_CreateWindow("CylSim", SCREEN_X, SCREEN_Y, SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            if(ptr == null)
            {
                Log.Instance.Debug($"Failed to create window! SDL error: {SDL.SDL_GetError()}");
                throw new InvalidOperationException(SDL.SDL_GetError());
            }
        }

        public void Update()
        {
            SDLRenderer.Instance.Clear();
        }

        public void Render()
        {
            SDLRenderer.Instance.Present();
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

                SDL.SDL_DestroyWindow(ptr);
                SDL.SDL_Quit();
                disposedValue = true;
            }
        }

        ~SDLWindow()
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
