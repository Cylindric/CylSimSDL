using Engine.Models.Import;
using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Engine.Models
{
    /// <summary>
    /// Represents an image containing one or more individual sprites.
    /// </summary>
    internal class SpriteSheet
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private SDLTexture _texture;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */
        public SpriteSheet()
        {
            _sprites = new Dictionary<string, Sprite>();
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public string Name { get; set; }
        public string SortingLayer { get; set; }
        public Dictionary<string, Sprite> _sprites;

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        public void Load(string filepath)
        {
            Log.Instance.Debug($"Loading Spritesheet {filepath}.");

            // First get the data about the sprites
            var filestream = new StreamReader(filepath);
            var serializer = new XmlSerializer(typeof(XmlTextureAtlas));
            var atlas = (XmlTextureAtlas)serializer.Deserialize(filestream);

            // Full filename information
            var imagefile = Path.Combine(Path.GetDirectoryName(filepath), atlas.imagePath);

            // Now load the image itself
            _texture = new SDLTexture();
            _texture.Load(imagefile);

            // Set up the basic SpriteSheet data.
            Name = Path.GetFileNameWithoutExtension(atlas.imagePath);

            // Add each sprite-set to the sheet.
            foreach (var data in atlas.Sprites)
            {
                var sprite = new Sprite(_texture)
                {
                    Name = data.name,
                    X = data.x,
                    Y = data.y,
                    Width = data.width,
                    Height = data.height,
                    Px = data.pivotX,
                    Py = data.pivotY
                };

                _sprites.Add(sprite.Name, sprite);
            }
        }

        public void Render(Sprite sprite, int x, int y, int width, int height)
        {
            _texture.RenderSprite(sprite, x, y, width, height);
        }
    }
}