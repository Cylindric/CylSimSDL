using Engine.Interfaces;
using Engine.Utilities;

namespace Engine.Controllers
{
    public class CameraController : ICameraController
    {
        public ICameraController Main => throw new System.NotImplementedException();

        public ICameraController Transform => throw new System.NotImplementedException();

        public Vector3 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
