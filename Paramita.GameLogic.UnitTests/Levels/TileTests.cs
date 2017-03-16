using NUnit.Framework;
using NUnit.Framework.Constraints;
using Paramita.GameLogic.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.UnitTests.Levels
{
    [TestFixture]
    public class TileTests
    {
        #region Test TileType.Set() Logic
        [Test]
        public void SetTileType_FloorOrStairs_IsWalkableIsTransparentFlagsTrue()
        {
            // constructor calls TileType.Set()
            var tiles = new List<Tile>();
            tiles.Add(new Tile(10, 10, TileType.Floor));
            tiles.Add(new Tile(10, 10, TileType.StairsUp));
            tiles.Add(new Tile(10, 10, TileType.StairsDown));

            foreach(var tile in tiles)
            {
                Assert.True(tile.IsWalkable);
                Assert.True(tile.IsTransparent);
            }
        }

        [Test]
        public void SetTileType_DoorOrWall_IsWalkableIsTransparentFlagsFalse()
        {
            // constructor calls TileType.Set()
            var tiles = new List<Tile>();
            tiles.Add(new Tile(10, 10, TileType.Door));
            tiles.Add(new Tile(10, 10, TileType.Wall));

            foreach(var tile in tiles)
            {
                Assert.False(tile.IsWalkable);
                Assert.False(tile.IsTransparent);
            }
        }

        [Test]
        public void SetTileType_OtherTileType_ThrowsNotImplementedException()
        {
            ActualValueDelegate<object> test = () => new Tile(10, 10, TileType.None);

            Assert.That(test, Throws.TypeOf<NotImplementedException>());
        }
        #endregion
    }
}
