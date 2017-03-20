using Microsoft.Xna.Framework;
using NUnit.Framework;
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


        #region Helper Methods
        private Tile MakeTile(int x, int y)
        {
            return new Tile(x, y, TileType.Floor);
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
        #endregion
    }
}
