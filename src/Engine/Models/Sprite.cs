using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using System.Diagnostics;

namespace Engine.Models
{
    [DebuggerDisplay("{Name} ({Width} × {Height})")]
    internal class Sprite
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private SpriteClip _clip = new SpriteClip();

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */
        public Sprite(SDLTexture texture)
        {
            Texture = texture;
            Centered = false;
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public SDLTexture Texture { get; set; }
        public string Name { get; internal set; }

        public int X
        {
            get
            {
                return _clip.X;
            }
            internal set
            {
                _clip.X = value;
            }
        }

        public int Y
        {
            get
            {
                return _clip.Y;
            }
            internal set
            {
                _clip.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return _clip.Width;
            }
            internal set
            {
                _clip.Width = value;
            }
        }

        public int Height
        {
            get
            {
                return _clip.Height;
            }
            internal set
            {
                _clip.Height = value;
            }
        }

        public float Px { get; internal set; }
        public float Py { get; internal set; }

        public SpriteClip Clip {
            get {
                return _clip;
            }
        }

        public bool Centered { get; internal set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
    }
}
