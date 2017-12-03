using Engine.Renderer;
using Engine.Utilities;

namespace Engine.Models
{
    internal class GameObject
    {
        public Sprite sprite { get; set; }
        public string name { get; set; }
        public Vector3 position { get; set; }
        public SpriteSheet SpriteSheet { get; internal set; }
        public Sprite ActiveSprite { get; set; }
    }
}