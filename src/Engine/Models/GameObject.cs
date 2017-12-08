using Engine.Renderer;
using Engine.Utilities;
using System.Diagnostics;

namespace Engine.Models
{
    [DebuggerDisplay("{Name}")]
    internal class GameObject
    {
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
        public Sprite Sprite { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// The position of this GameObject in World coordinates (Tile XY)
        /// </summary>
        public Vector2<float> Position { get; set; }

        public SpriteSheet SpriteSheet { get; internal set; }
        public Sprite ActiveSprite { get; set; }
        public float Rotation { get; set; }
        public bool IsActive { get; set; }
        public SimplePool.Pool Pool { get; set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        /// <summary>
        /// Creates a copy of the supplied GameObject prefab.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObject Instantiate(GameObject prefab)
        {
            return new GameObject()
            {
                Sprite = prefab.Sprite,
                Name = prefab.Name,
                Position = prefab.Position,
                SpriteSheet = prefab.SpriteSheet,
                ActiveSprite = prefab.ActiveSprite,
                Rotation = prefab.Rotation,
                IsActive = prefab.IsActive
            };
        }

        /// <summary>
        /// Creates a copy of the supplied GameObject prefab.
        /// </summary>
        /// <param name="prefab">The GameObject to spawn.</param>
        /// <param name="worldPos">The world-space coordinates for this object.</param>
        /// <returns></returns>
        public static GameObject Instantiate(GameObject prefab, Vector2<float> worldPos)
        {
            var go = Instantiate(prefab);
            go.Position = worldPos;
            return go;
        }

        /// <summary>
        /// Creates a copy of the supplied GameObject prefab.
        /// </summary>
        /// <param name="prefab">The GameObject to spawn.</param>
        /// <param name="worldPos">The world-space coordinates for this object.</param>
        /// <param name="rotation">The rotation of the sprite.</param>
        /// <returns></returns>
        public static GameObject Instantiate(GameObject prefab, Vector2<float> worldPos, float rotation)
        {
            var go = Instantiate(prefab, worldPos);
            go.Rotation = rotation;
            return go;
        }

    }
}