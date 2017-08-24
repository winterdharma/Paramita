using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Scenes.Game
{
    public class Image : Element
    {
        public Image(string id, Component parent, Vector2 position) : base(id, parent, position)
        {

        }

        public string Label { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get; set; }
        public Color Color { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        protected override Rectangle CreateRectangle()
        {
            throw new System.NotImplementedException();
        }
    }
}
