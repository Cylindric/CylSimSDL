using Engine.Renderer;
using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Engine.Models
{
    internal class Text
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private SDLText _text;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */
        public Text()
        {
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        public Vector2<int> Position { get; set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
        public void Create(string text, string font, int size, SDL_Color colour)
        {
            Position = new Vector2<int>();
            _text = new SDLText();
            _text.Create(text, font, size, colour);
        }

        public void Render()
        {
            _text.Position = Position;
            _text.Render();
        }
    }
}
