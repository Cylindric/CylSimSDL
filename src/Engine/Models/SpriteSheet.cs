﻿using Engine.Models.Import;
using Engine.Renderer.SDLRenderer;
using Engine.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System;

namespace Engine.Models
{
    /// <summary>
    /// Represents an image containing one or more individual sprites.
    /// </summary>
    [DebuggerDisplay("{Name}")]
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
            Sprites = new Dictionary<string, Sprite>();
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public string Name { get; set; }
        public string SortingLayer { get; set; }
        public Dictionary<string, Sprite> Sprites;

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

                Sprites.Add(sprite.Name, sprite);
            }
        }

        /// <summary>
        /// Render the given sprite at the specified screen location and size.
        /// </summary>
        /// <param name="sprite">The sprite to render.</param>
        /// <param name="screenPos">The screen-coordinate to render at.</param>
        /// <param name="width">The width to render the sprite.</param>
        /// <param name="height">The height to render the sprite.</param>
        internal void Render(Sprite sprite, Vector2<int> screenPos)
        {
            Render(sprite, screenPos.X, screenPos.Y);
        }

        public void Render(Sprite sprite, int x, int y)
        {
            if (sprite.Centered)
            {
                x = x - (sprite.Width / 2);
                y = y - (sprite.Width / 2);
            }
            int width = sprite.Width;
            int height = sprite.Height;
            _texture.RenderSprite(sprite, x, y, width, height);
        }
    }
}