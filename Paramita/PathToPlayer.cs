using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI;
using RogueSharp;
using System.Collections.Generic;
using System.Linq;

namespace Paramita
{
    public class PathToPlayer
    {
        private readonly Player _player;
        private readonly IMap _map;
        private readonly Texture2D _sprite;
        private readonly PathFinder _pathFinder;
        private Path _path;
        private IEnumerable<Cell> _cells;

        public PathToPlayer(Player player, IMap map, Texture2D sprite)
        {
            _player = player;
            _map = map;
            _sprite = sprite;
            _pathFinder = new PathFinder(map);
        }
        public Cell FirstCell
        {
            get
            {
                return _cells.First();
            }
        }
        public void CreateFrom(int x, int y)
        {
            _path = _pathFinder.ShortestPath(_map.GetCell(x, y),
              _map.GetCell(_player.X, _player.Y));
            _cells = _path.Steps;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_cells != null && Global.GameState == OldGameStates.Debugging)
            {
                foreach (Cell cell in _cells)
                {
                    if (cell != null)
                    {
                        float scale = .25f;
                        float multiplier = .25f * _sprite.Width;
                        spriteBatch.Draw(_sprite, new Vector2(cell.X * multiplier, cell.Y * multiplier),
                          null, null, null, 0.0f, new Vector2(scale, scale), Color.Blue * .2f,
                          SpriteEffects.None, LayerDepth.Paths);
                    }
                }
            }
        }
    }
}
