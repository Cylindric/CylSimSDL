using Engine.Models;
using Engine.Models.Import;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.Controllers
{
    internal class SpriteManager
    {
        #region Singleton
        private static readonly Lazy<SpriteManager> _instance = new Lazy<SpriteManager>(() => new SpriteManager());

        public static SpriteManager Instance { get { return _instance.Value; } }

        private SpriteManager()
        {
            Start();
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */
        private const int PPU = 64;

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private readonly Dictionary<string, SpriteSheet> _sprites = new Dictionary<string, SpriteSheet>();

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        public Sprite GetSprite(string atlasName, string spriteName)
        {
            if (_sprites.ContainsKey(atlasName) == false || _sprites[atlasName].Sprites.ContainsKey(spriteName) == false)
            {
                Log.Instance.Debug($"No sprite with name {spriteName} in atlas {atlasName}!");
                return null;
            }
            return _sprites[atlasName].Sprites[spriteName];
        }

        private void Start()
        {
            LoadSprites();
        }

        private void LoadSprites()
        {
            var filepath = Engine.Instance.Path("base", "images");
            LoadSprites(filepath);
        }

        private void LoadSprites(string filepath)
        {
            foreach (var dir in Directory.GetDirectories(filepath))
            {
                LoadSprites(dir);
            }

            foreach (var file in Directory.GetFiles(filepath).Where(f => f.EndsWith(".xml")))
            {
                LoadSpritesheet(file);
            }
        }

        private void LoadSpritesheet(string filepath)
        {
            // Set up the basic SpriteSheet data.
            var spritesheet = new SpriteSheet();
            spritesheet.Load(filepath);
            _sprites.Add(spritesheet.Name, spritesheet);
        }
    }
}
