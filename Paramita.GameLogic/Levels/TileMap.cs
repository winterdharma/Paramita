﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;
using Paramita.GameLogic.Utility;

namespace Paramita.GameLogic.Levels
{
    public class TileMap : IEquatable<TileMap>
    {
        #region Fields
        private Tile[,] _tiles;
        private int _tilesWide;
        private int _tilesHigh;
        private string _mapName;
        private const string _unsupportedTileType = "A TileType was supplied that is not supported for this search.";
        #endregion


        #region Events
        public event EventHandler<ItemEventArgs> OnItemAdded;
        public event EventHandler<ItemEventArgs> OnItemRemoved;
        #endregion


        public TileMap(Tile[,] tiles, string name)
        {
            _tiles = tiles;
            _tilesWide = tiles.GetLength(0);
            _tilesHigh = tiles.GetLength(1);
            MapName = name;
            SubscribeToTileEvents();
        }

        #region Properties
        public Tile[,] Tiles
        {
            get { return _tiles; }
            private set { _tiles = value; }
        }

        public int TilesWide
        {
            get { return _tilesWide; }
        }

        public int TilesHigh
        {
            get { return _tilesHigh; }
        }

        public string MapName
        {
            get { return _mapName; }
            private set { _mapName = value; }
        }
        #endregion


        #region Event Handlers
        private void HandleItemAddedEvent(object sender, ItemEventArgs e)
        {
            OnItemAdded?.Invoke(this, e);
        }

        private void HandleItemRemovedEvent(object sender, ItemEventArgs e)
        {
            OnItemRemoved?.Invoke(this, e);
        }
        #endregion


        #region TileType and ItemType Layer API
        internal TileType[,] ConvertMapToTileTypes()
        {
            var typeArray = new TileType[_tilesHigh, _tilesWide];

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for(int j = 0; j < _tiles.GetLength(1); j++)
                {
                    typeArray[i, j] = _tiles[i, j].TileType;
                }
            }

            return typeArray;
        }


        internal ItemType[,] ConvertMapToItemTypes()
        {
            var typeArray = new ItemType[_tilesHigh, _tilesWide];

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    var items = _tiles[i, j].InspectItems();
                    typeArray[i, j] = ConvertItemsToItemType(items);
                }
            }

            return typeArray;
        }
        #endregion


        #region Tile Getter and Setter API
        // This setter ensures that the TilePoints and index positions in _tiles
        // always correspond. The property has a private set() to prevent incongruity.
        // Using the TilePoint for array indices also serves as a null check.
        public void SetTile(Tile newTile)
        {
            Tiles[newTile.TilePoint.X, newTile.TilePoint.Y] = newTile;
        }

        // This getter is for convenience to retrieve tiles using a point rather
        // than a pair of ints.
        public Tile GetTile(Point point)
        {
            return _tiles[point.X, point.Y];
        }

        public Tile GetRandomWalkableTile()
        {
            var walkableTiles = GetWalkableTiles();

            int i = Dungeon._random.Next(walkableTiles.Count - 1);
                
            return walkableTiles[i];
        }

        public void PlaceItemsOnRandomTiles(List<Item> items)
        {
            foreach(var item in items)
            {
                var tile = GetRandomWalkableTile();
                tile.AddItem(item);
            }
        }
        #endregion

        public Tile FindTileType(TileType type)
        {
            switch(type)
            {
                case TileType.StairsUp:
                    return FindTileByType(TileType.StairsUp);
                case TileType.StairsDown:
                    return FindTileByType(TileType.StairsDown);
                default:
                    throw new NotImplementedException("Finding TileType." + type + " not implemented.");
            }
        }

        #region IEquatable
        public bool Equals(TileMap other)
        {
            if (TilesAreEqual(_tiles, other.Tiles) && _mapName.Equals(other.MapName))
                return true;
            return false;

        }

        private bool TilesAreEqual(Tile[,] these, Tile[,] those)
        {
            if (!ArraySizeIsEqual(these, those))
                return false;

            if (!ArrayTilesAreEqual(these, those))
                return false;

            return true;
        }

        private bool ArraySizeIsEqual(Tile[,] first, Tile[,] second)
        {
            if (first.GetLength(0) != second.GetLength(0) || first.GetLength(1) != second.GetLength(1))
                return false;
            return true;
        }
        
        private bool ArrayTilesAreEqual(Tile[,] first, Tile[,] second)
        {
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    if (first[i, j] != second[i, j])
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region Helper Methods
        private ItemType ConvertItemsToItemType(Item[] items)
        {
            if (items.Length == 0)
                return ItemType.None;
            else
                return items[0].ItemType;
        }

        private List<Tile> GetWalkableTiles()
        {
            var queryResults = from Tile tile in _tiles
                               where tile.IsWalkable == true
                               select tile;

            if (queryResults.Count() == 0)
                throw new NullReferenceException("No walkable tiles found.");

            return queryResults.ToList();
        }

        private void SubscribeToTileEvents()
        {
            foreach (var tile in _tiles)
            {
                tile.OnItemAddedToTile += HandleItemAddedEvent;
                tile.OnItemRemovedFromTile += HandleItemRemovedEvent;
            }
        }

        // conducts a linear search of @tiles and returns the first match encountered
        private Tile FindTileByType(TileType type)
        {
            for(int x = 0; x < TilesWide; x++)
            {
                for(int y = 0; y < TilesHigh; y++)
                {
                    if (_tiles[x, y].TileType == type)
                        return _tiles[x, y];
                }
            }
            throw new NullReferenceException("No matching tile found.");
        }
        #endregion
    }
}
