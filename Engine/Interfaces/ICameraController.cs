using Engine.Utilities;

namespace Engine.Interfaces
{
    public interface ICameraController
    {
        ICameraController Main { get; }
        ICameraController Transform { get; }
        Vector3 Position { get; set; }
    }
}
