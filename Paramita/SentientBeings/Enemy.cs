using Microsoft.Xna.Framework.Graphics;
using RogueSharp;


namespace Paramita.SentientBeings
{
    public class Enemy //: SentientBeing
    {/*
        private readonly IMap _map;
        private bool _isAwareOfPlayer;
        private readonly PathToPlayer _path;
        

        public Enemy(GameController game, IMap map, PathToPlayer path) : base(game)
        {
            _map = map;
            _path = path;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Height),
            //    null, null, null, 0.0f, Vector2.One, Color.White, 
            //    SpriteEffects.None, LayerDepth.Sprites);
            //_path.Draw(spriteBatch);
        }

        public void Update()
        {
            if (_isAwareOfPlayer == false)
            {
                if (_map.IsInFov(X, Y))
                {
                    _isAwareOfPlayer = true;
                }
            }

            if (_isAwareOfPlayer)
            {
                _path.CreateFrom(X, Y);
                // Use CombatManager to check if player occupies cell will move  into
                if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y) == true)
                {
                    Global.CombatManager.Attack(this,
                        Global.CombatManager.SattvaAt(_path.FirstCell.X, _path.FirstCell.Y));
                }
                else
                {
                    X = _path.FirstCell.X;
                    Y = _path.FirstCell.Y;
                }
            }
        }
    */}
}
