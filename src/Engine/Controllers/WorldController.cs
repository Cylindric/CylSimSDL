﻿using System;
using Engine.Models;
using Engine.Utilities;

namespace Engine.Controllers
{
    internal class WorldController
    {
        #region Singleton
        private static readonly Lazy<WorldController> _instance = new Lazy<WorldController>(() => new WorldController());

        public static WorldController Instance { get { return _instance.Value; } }

        private WorldController()
        {
            if (_loadWorld)
            {
                CreateWorldFromSave();
            }
            else
            {
                CreateEmptyWorld();
            }
        }
        #endregion

        private static bool _loadWorld = false;

        private void CreateEmptyWorld()
        {
            Log.Instance.Debug("Creating empty world.");
            World.Instance.Generate(100, 100);

            TileSpriteController.Instance.Setup();

            World.Instance.CreateCharacter(World.Instance.GetTileAt(World.Instance.Width / 2, World.Instance.Height / 2)); // First test character

            CameraController.Instance.Position = new Vector3(World.Instance.Width / 2f, World.Instance.Height / 2f, CameraController.Instance.Position.Z);
        }

        private void CreateWorldFromSave()
        {
            throw new NotImplementedException();
        }

        public Tile GetTileAtWorldCoordinates(Vector3 coord)
        {
            var x = Mathf.FloorToInt(coord.X);
            var y = Mathf.FloorToInt(coord.Y);

            return World.Instance.GetTileAt(x, y);
        }

        public void Update()
        {
            World.Instance.Update(Time.DeltaTime);
        }

        public void Render()
        {
            TileSpriteController.Instance.Render();
        }
    }
}