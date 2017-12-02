using Engine.Models;

namespace Engine.Interfaces
{
    internal interface IWorld
    {
        int Width { get; }
        int Height { get; }

        Tile GetTileAt(int x, int y);
        void Generate(int width, int height);
        Character CreateCharacter(Tile tile);
        void Update(float deltaTime);
    }
}
