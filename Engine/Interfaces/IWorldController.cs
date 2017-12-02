using Engine.Models;
using Engine.Utilities;

namespace Engine.Interfaces
{
    internal interface IWorldController
    {
        void Update();
        Tile GetTileAtWorldCoordinates(Vector3 coord);
    }
}
