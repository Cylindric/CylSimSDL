using Engine.Interfaces;
using SDL2;
using System;

namespace Engine.Renderer
{
    public class SDLWindow : IWindow
    {
        private const int SCREEN_X = 100;
        private const int SCREEN_Y = 100;
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 600;

        private IntPtr _window;
        private IntPtr _renderer;

        public IntPtr Renderer => _renderer;
        public IntPtr Window => _window;

        public void Start()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            _window = SDL.SDL_CreateWindow("CylSim", SCREEN_X, SCREEN_Y, SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            _renderer = SDL.SDL_CreateRenderer(_window, -1, 0);
        }

        public void Update()
        {
            SDL.SDL_RenderClear(_renderer);
        }

        public void Render()
        {
            SDL.SDL_RenderPresent(_renderer);
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

                SDL.SDL_DestroyRenderer(_renderer);
                SDL.SDL_DestroyWindow(_window);
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
