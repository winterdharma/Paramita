using Microsoft.Xna.Framework;
using NUnit.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Levels
{
    [TestFixture]
    public class TileMapTests
    {
        #region GetTile Tests
        [Test]
        public void GetTile_GivenValidIndices_ReturnsTileWithSameTilePoint()
        {
            var tileMap = MakeTileMap();
            var expected = new Point(0, 2);
            var actual = tileMap.GetTile(expected).TilePoint;

            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region SetTile Tests
        [Test]
        public void SetTile_GivenValidTile_SetsTileAtArrayIndicesSameAsTilePoint()
        {
            var tileMap = MakeTileMap();
            var expected = MakeTile(0, 1);
            tileMap.SetTile(expected);

            var actual = tileMap.Tiles[0, 1];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetTile_NullTile_ThrowsException()
        {
            var tileMap = MakeTileMap();
            Assert.Catch<NullReferenceException>(() => tileMap.SetTile(null));
        }
        #endregion

        #region GetRandomWalkableTile Tests
        [Test]
        public void GetRandomWalkableTile_ReturnsWalkableTiles()
        {
            var tileMap = MakeTileMap();
            var actual = new List<bool>();

            for(int i = 0; i < 100; i++)
            {
                actual.Add(tileMap.GetRandomWalkableTile().IsWalkable);
            }

            CollectionAssert.DoesNotContain(actual, false);
        }
        #endregion

        #region PlaceItemsOnRandomTiles Tests
        [Test]
        public void PlaceItemsOnRandomTiles_AllItemsPlaced()
        {
            var tileMap = MakeTileMap();
            var expected = MakeItems();

            tileMap.PlaceItemsOnRandomTiles(expected);
            var actual = new List<Item>(GetItemsOnMap(tileMap));

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void PlaceItemsOnRandomTiles_PlacedOnWalkableTiles()
        {
            var tileMap = MakeTileMap();
            var items = MakeItems();

            tileMap.PlaceItemsOnRandomTiles(items);

            var tiles = GetTilesWithItems(tileMap);
            
            var actual = new List<bool>();
            foreach (var tile in tiles)
            {
                actual.Add(tile.IsWalkable);
            }

            CollectionAssert.DoesNotContain(actual, false);
        }
        #endregion

        #region FindTileType Tests
        [TestCase(0, 2, TileType.StairsUp)]
        [TestCase(1, 0, TileType.StairsDown)]
        public void FindTileType_GivenValidTileType_FindsTileOfThatType(int x, int y, TileType type)
        {
            var tileMap = MakeTileMap();
            var expected = new Tile(x, y, type);

            var actual = tileMap.FindTileType(type);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindTileType_WhenNoTileMatches_ThrowsException()
        {
            var tileMap = MakeTileMap();
            tileMap.SetTile(new Tile(0, 2, TileType.Floor));

            Assert.Catch<NullReferenceException>(() => tileMap.FindTileType(TileType.StairsUp));
        }

        [TestCase(TileType.Floor)]
        [TestCase(TileType.Door)]
        [TestCase(TileType.None)]
        [TestCase(TileType.Wall)]
        public void FindTileType_ThrowsExceptionUnimplementedTypes(TileType type)
        {
            var tileMap = MakeTileMap();
            NotImplementedException e = null;

            e = Assert.Catch<NotImplementedException>(() => tileMap.FindTileType(type));
            StringAssert.Contains(type.ToString(), e.Message);
        }
        #endregion

        #region Helper Methods
        private Tile MakeTile(int x, int y)
        {
            return new Tile(x, y, TileType.Floor);
        }

        private List<Item> MakeItems()
        {
            return new List<Item>()
            {
                ItemCreator.CreateShortSword(),
                ItemCreator.CreateBuckler(),
                ItemCreator.CreateMeat()
            };
        }

        private TileMap MakeTileMap()
        {
            var tiles = new Tile[3, 3]
            {
                {
                    new Tile(0,0, TileType.Wall),
                    new Tile(0,1, TileType.Wall),
                    new Tile(0,2, TileType.StairsUp)
                },
                {
                    new Tile(1,0, TileType.StairsDown),
                    new Tile(1,1, TileType.Wall),
                    new Tile(1,2, TileType.Wall)
                },
                {
                    new Tile(2,0, TileType.Floor),
                    new Tile(2,1, TileType.Floor),
                    new Tile(2,2, TileType.Floor)
                }
            };
            return new TileMap(tiles, "3by3_testmap");
        }

        internal static List<Item> GetItemsOnMap(TileMap map)
        {
            var items = new List<Item>();
            for (int x = 0; x < map.TilesWide; x++)
            {
                for (int y = 0; y < map.TilesHigh; y++)
                {
                    items.AddRange(map.GetTile(new Point(x, y)).InspectItems());
                }
            }
            return items;
        }

        private List<Tile> GetTilesWithItems(TileMap map)
        {
            var tiles = new List<Tile>();
            for (int x = 0; x < map.TilesWide; x++)
            {
                for (int y = 0; y < map.TilesHigh; y++)
                {
                    var point = new Point(x, y);
                    if (map.GetTile(point).InspectItems().Length > 0)
                    {
                        tiles.Add(map.GetTile(point));
                    }
                }
            }
            return tiles;
        }
        #endregion
    }
}
