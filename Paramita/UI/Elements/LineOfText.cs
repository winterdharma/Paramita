using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Base;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// A class representing a single-line text element, such as a message or label.
    /// </summary>
    public class LineOfText : Element
    {        
        public string Text { get; set; }
        public SpriteFont Font { get; set; }

        public LineOfText(string id, Component parent, Vector2 position, string text, 
            SpriteFont font, Color unhighlighted, Color highlighted, int drawOrder) 
            : base(id, parent, position, unhighlighted, highlighted, drawOrder)
        {
            Id = id;
            Text = text;
            Font = font;
            Position = position;
            Rectangle = CreateRectangle();
            DrawOrder = drawOrder;
        }

        protected override Rectangle CreateRectangle()
        {
            var size = Font.MeasureString(Text);
            return new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if(Enabled)
            {

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Visible)
            {
                spriteBatch.DrawString(Font, Text, Position, Color);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (Visible)
            {
                spriteBatch.DrawString(Font, Text, position, Color);
            }
        }

        /// <summary>
        /// Calculates the horizontally and veritcally centered position for the LineOfText given a 
        /// container rectangle. This method assumes that the container rectangle has a absolute location 
        /// that places it on the app screen.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public Vector2 GetCenteredPosition(Rectangle container)
        {
            var textSize = Rectangle.Size;
            var containerSize = container.Size;
            return new Vector2(
                (container.X + containerSize.X / 2 - textSize.X / 2),
                (container.Y + containerSize.Y / 2 - textSize.Y / 2)
                );
        }
    }
}
