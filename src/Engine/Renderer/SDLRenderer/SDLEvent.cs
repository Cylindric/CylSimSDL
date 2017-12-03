using Engine.Utilities;
using SDL2;
using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace Engine.Renderer.SDLRenderer
{
    internal static class SDLEvent
    {
        static SDLEvent()
        {
            _keyEvents = new List<SDL.SDL_Event>();
            _events = new List<SDL.SDL_Event>();
            _downKeys = new List<SDL_Keycode>();
            Quit = false;
        }

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private static List<SDL.SDL_Event> _keyEvents;
        private static List<SDL.SDL_Event> _events;
        private static List<SDL_Keycode> _downKeys;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public static bool Quit { get; private set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
        public static void Update()
        {
            _keyEvents.Clear();
            _events.Clear();
            Quit = false;

            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    Quit = true;
                }

                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    if (_downKeys.Contains(e.key.keysym.sym) == false)
                    {
                        _downKeys.Add(e.key.keysym.sym);
                    }
                    _keyEvents.Add(e);
                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    if (_downKeys.Contains(e.key.keysym.sym))
                    {
                        _downKeys.Remove(e.key.keysym.sym);
                    }
                    _keyEvents.Add(e);
                }
                else
                {
                    _events.Add(e);
                }
            }
        }

        public static bool KeyState(SDL_Keycode key)
        {
            return _downKeys.Contains(key);
        }

        public static bool KeyUp(SDL_Keycode key)
        {
            foreach(var k in _keyEvents)
            {
                if (k.key.keysym.sym == key)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
