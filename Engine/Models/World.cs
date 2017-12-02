using Engine.Interfaces;
using Engine.Pathfinding;
using Engine.Utilities;
using System;
using System.Collections.Generic;

namespace Engine.Models
{
    internal class World : IWorld
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */

        private Dictionary<string, Furniture> _furniturePrototypes;
        private List<Character> _characters;
        private List<Furniture> _furnitures;
        private List<Room> _rooms;
        private Tile[,] _tiles;
        private ILog _log;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        public World(ILog log)
        {
            this._log = log;
            this.JobQueue = new JobQueue();
            this._characters = new List<Character>();
            this._furnitures = new List<Furniture>();

            this._rooms = new List<Room>
            {
                new Room() // Add the default 'outside' room.
            };
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        private Action<Furniture> _cbFurnitureCreated;
        private Action<Character> _cbCharacterCreated;
        private Action<Tile> _cbTileChanged;

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /// <summary>
        /// The width of the world, in Tiles.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the world, in Tiles.
        /// </summary>
        public int Height { get; private set; }

        public Path_TileGraph TileGraph { get; set; } // TODO: this PathTileGraph really shouldn't be fully public like this.

        public JobQueue JobQueue { get; set; }

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        public void Generate(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this._tiles = new Tile[width, height];

            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    this._tiles[x, y] = new Tile(this, x, y);
                    this._tiles[x, y].RegisterTileTypeChangedCallback(this.OnTileChanged);
                    this._tiles[x, y].Room = _rooms[0];
                }
            }

            _log.Debug("World (" + this.Width + "," + this.Height + ") created with " + (this.Width * this.Height) + " tiles.");

            this.CreateFurniturePrototypes();
        }

        public Furniture PlaceFurniture(string objectType, Tile t)
        {
            if (this._furniturePrototypes.ContainsKey(objectType) == false)
            {
                //Debug.LogErrorFormat("Tried to place an object [{0}] for which we don't have a prototype.", objectType);
                return null;
            }

            var furn = Furniture.PlaceInstance(this._furniturePrototypes[objectType], t);

            if (furn == null)
            {
                // Failed to place object! Maybe something was already there.
                return null;
            }

            this._furnitures.Add(furn);

            // Recalculate rooms?
            if (furn.IsRoomEnclosure)
            {
                Room.DoRoomFloodfill(furn);
            }

            if (this._cbFurnitureCreated != null)
            {
                this._cbFurnitureCreated(furn);

                if (Mathf.Approximately(furn.MovementCost, 1f) == false)
                {
                    // Tiles with a movement cost of exactly 1, don't affect the path-finding for their tile.
                    this.InvalidateTileGraph();
                }
            }

            return furn;
        }

        public void RegisterFurnitureCreatedCb(Action<Furniture> cb)
        {
            this._cbFurnitureCreated += cb;
        }

        public void UnRegisterFurnitureCreatedCb(Action<Furniture> cb)
        {
            this._cbFurnitureCreated -= cb;
        }

        public void RegisterCharacterCreatedCb(Action<Character> cb)
        {
            this._cbCharacterCreated += cb;
        }

        public void UnRegisterCharacterCreatedCb(Action<Character> cb)
        {
            this._cbCharacterCreated -= cb;
        }

        public void RegisterTileChanged(Action<Tile> cb)
        {
            this._cbTileChanged += cb;
        }

        public void UnRegisterTileChanged(Action<Tile> cb)
        {
            this._cbTileChanged -= cb;
        }

        public bool IsFurniturePlacementValid(string furnitureType, Tile t)
        {
            return this._furniturePrototypes[furnitureType].IsValidPosition(t);
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x >= this.Width || x < 0 || y >= this.Height || y < 0)
            {
                return null;
            }

            return this._tiles[x, y];
        }

        public Character CreateCharacter(Tile t)
        {
            var c = new Character(t);
            this._characters.Add(c);

            if (this._cbCharacterCreated != null)
            {
                this._cbCharacterCreated(c);
            }

            return c;
        }

        public void Update(float deltaTime)
        {
            foreach (var c in this._characters)
            {
                c.Update(deltaTime);
            }

            foreach (var f in this._furnitures)
            {
                f.Update(deltaTime);
            }
        }

        public Room GetOutsideRoom()
        {
            return _rooms[0];
        }

        public void AddRoom(Room r)
        {
            _rooms.Add(r);
        }

        public void DeleteRoom(Room r)
        {
            if (r == GetOutsideRoom())
            {
                //Debug.LogError("Tried to delete the outside room!");
                return;
            }

            // Remove the current room from the list of rooms.
            _rooms.Remove(r);

            // Make sure no tiles point to this room.
            // TODO: This probably isn't necessary, as the flood-fill will assign all these tiles to new rooms.
            r.UnassignAllTiles();
        }

        public void SetupPathfindingTestMap()
        {
            var hMid = this.Width / 2;
            var vMid = this.Height / 2;

            for (int x = 5; x < this.Width - 5; x++)
            {
                for (int y = 5; y < this.Height - 5; y++)
                {
                    this._tiles[x, y].Type = TileType.Floor;

                    // Place some walls
                    if ((x == hMid - 3 || x == hMid + 3) || (y == vMid - 3 || y == vMid + 3))
                    {
                        if (x == hMid || y == vMid)
                        {

                        }
                        else
                        {
                            this.PlaceFurniture("Wall", this._tiles[x, y]);
                        }
                    }
                }
            }
        }

        private void OnTileChanged(Tile t)
        {
            this._cbTileChanged?.Invoke(t);

            this.InvalidateTileGraph();
        }

        /// <summary>
        /// Invalidates the current TileGraph.
        /// </summary>
        /// <remarks>
        /// Should be called whenever anything changes that affects the pathing.</remarks>
        private void InvalidateTileGraph()
        {
            this.TileGraph = null;
        }

        private void CreateFurniturePrototypes()
        {
            this._furniturePrototypes = new Dictionary<string, Furniture>();

            this._furniturePrototypes.Add("Wall", new Furniture("Wall", 0f, 1, 1, true, true));
            this._furniturePrototypes.Add("Door", new Furniture("Door", 2f, 1, 1, false, true));

            this._furniturePrototypes["Door"].SetParameter("openness", 0.0f);
            this._furniturePrototypes["Door"].SetParameter("is_opening", 0.0f);
            this._furniturePrototypes["Door"].RegisterUpdateAction(FurnitureActions.Door_UpdateAction);
            this._furniturePrototypes["Door"].IsEntereable = FurnitureActions.Door_IsEnterable;
        }

    }
}