﻿using Engine.Models;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Controllers
{
    internal class TileSpriteController
    {
        #region Singleton
        private static readonly Lazy<TileSpriteController> _instance = new Lazy<TileSpriteController>(() => new TileSpriteController());

        public static TileSpriteController Instance { get { return _instance.Value; } }

        private TileSpriteController()
        {
        }
        #endregion

        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */
        const int GRID_SIZE = 64;

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */
        private readonly Dictionary<Tile, GameObject> _tileGameObjectMap = new Dictionary<Tile, GameObject>();

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */


        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */
        public Sprite FloorSprite { get; set; }
        public Sprite EmptySprite { get; set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */
        public void Setup()
        {
            // Load the Spritesheet for the tiles
            var spritesheet = new SpriteSheet();
            spritesheet.Load(Engine.Instance.Path("assets", "base", "tiles", "floor.xml"));

            FloorSprite = spritesheet._sprites.First().Value;
            EmptySprite = spritesheet._sprites.First().Value;

            // Create a game object for every Tile.
            for (var x = 0; x < World.Instance.Width; x++)
            {
                for (var y = 0; y < World.Instance.Height; y++)
                {
                    var tileData = World.Instance.GetTileAt(x, y);
                    var tileGo = new GameObject();
                    _tileGameObjectMap.Add(tileData, tileGo);

                    tileGo.name = "Tile_" + x + "_" + y;
                    tileGo.position = new Vector3(tileData.X, tileData.Y, 0);

                    tileGo.SpriteSheet = spritesheet;
                    tileGo.sprite = EmptySprite;
                    tileGo.SpriteSheet.SortingLayer = "Tiles";

                    OnTileChanged(tileData);
                }
            }

            World.Instance.RegisterTileChanged(OnTileChanged);
        }

        private void DestroyAllTileGameObjects()
        {
            while (_tileGameObjectMap.Count > 0)
            {
                var tileData = _tileGameObjectMap.Keys.First();
                var tileGo = _tileGameObjectMap[tileData];
                _tileGameObjectMap.Remove(tileData);
                tileData.UnRegisterTileTypeChangedCallback(OnTileChanged);
                // Destroy(tileGo);
            }
        }

        private void OnTileChanged(Tile tileData)
        {
            if (_tileGameObjectMap.ContainsKey(tileData) == false)
            {
                Log.Instance.Debug("TileGameObjectMap doesn't contain the tile_data.");
                return;
            }

            var tileGo = _tileGameObjectMap[tileData];

            if (tileGo == null)
            {
                Log.Instance.Debug("TileGameObjectMap returned a null GameObject.");
                return;
            }

            if (tileData.Type == TileType.Floor)
            {
                tileGo.sprite = FloorSprite;
            }
            else if (tileData.Type == TileType.Empty)
            {
                tileGo.sprite = EmptySprite;
            }
            else
            {
                Log.Instance.Debug("OnTileChanged - Unrecognised Tile type");
            }
        }

        public void Render()
        {
            foreach(var t in _tileGameObjectMap)
            {
                var go = t.Value;
                var posX = go.position.X * GRID_SIZE;
                var posY = go.position.Y * GRID_SIZE;

                if (go.position.X == 0 && go.position.Y == 0)
                {
                    //Log.Instance.Debug($"Tile 0,0 is going to be drawn at world coordinate {posX},{posY}.");
                }

                // offset render location by the camera movement
                posX -= CameraController.Instance.Position.X;
                posY += CameraController.Instance.Position.Y;

                if (go.position.X == 0 && go.position.Y == 0)
                {
                    Log.Instance.Debug($"Tile 0,0 is going to be drawn at screen coordinate {posX},{posY}.");
                }

                t.Value.SpriteSheet.Render(go.sprite, (int)posX, (int)posY, go.sprite.Width, go.sprite.Height);
            }
        }

    }
}