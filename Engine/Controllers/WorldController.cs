using System;
using Engine.Interfaces;
using Engine.Models;
using Engine.Utilities;

namespace Engine.Controllers
{
    internal class WorldController : IWorldController
    {
        private ILog _log;
        private ICameraController _camera;
        private ITime _time;

        private static bool _loadWorld = false;

        public IWorld World { get; private set; }

        public WorldController(IWorld world, ILog log, ICameraController camera, ITime time)
        {
            World = world;
            _log = log;
            _camera = camera;
            _time = time;

            if (_loadWorld)
            {
                CreateWorldFromSave();
            }
            else
            {
                CreateEmptyWorld();
            }
        }

        private void CreateEmptyWorld()
        {
            _log.Debug("Creating empty world.");
            this.World.Generate(100, 100);

            World.CreateCharacter(World.GetTileAt(World.Width / 2, World.Height / 2)); // First test character

            _camera.Main.Transform.Position = new Vector3(World.Width / 2f, World.Height / 2f, _camera.Main.Transform.Position.Z);
        }

        private void CreateWorldFromSave()
        {
            throw new NotImplementedException();
        }

        public Tile GetTileAtWorldCoordinates(Vector3 coord)
        {
            var x = Mathf.FloorToInt(coord.X);
            var y = Mathf.FloorToInt(coord.Y);

            return this.World.GetTileAt(x, y);
        }

        public void Update()
        {
            World.Update(_time.DeltaTime);

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Application.Quit();
            //}

            //float scrollSpeed = 4f;
            //if (Input.GetKey(KeyCode.A))
            //{
            //    _camera.Main.Transform.Position += Vector3.Left * Time.DeltaTime * scrollSpeed;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    _camera.Main.Transform.Position += Vector3.Right * Time.DeltaTime * scrollSpeed;
            //}
            //if (Input.GetKey(KeyCode.W))
            //{
            //    _camera.Main.Transform.Position += Vector3.Up * Time.DeltaTime * scrollSpeed;
            //}
            //else if (Input.GetKey(KeyCode.S))
            //{
            //    _camera.Main.Transform.Position += Vector3.Down * Time.DeltaTime * scrollSpeed;
            //}
        }
    }
}
