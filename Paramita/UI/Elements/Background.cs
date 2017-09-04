using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Utility;
using Paramita.UI.Base;

namespace Paramita.UI.Elements
{
    public class Background : Image
    {

        public Background(string id, Component parent, Vector2 position, Texture2D texture, Color color1, Color color2, Point size) 
            : base(id, parent, position, texture, color1, color2)
        {
            Rectangle = CreateRectangle(size);
            parent.RectangleUpdated += OnPanelRectangleUpdate;
        }

        private void OnPanelRectangleUpdate(object sender, RectangleEventArgs e)
        {
            Rectangle = e.Rectangle;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        protected Rectangle CreateRectangle(Point size)
        {
            return new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);
        }
    }
}
