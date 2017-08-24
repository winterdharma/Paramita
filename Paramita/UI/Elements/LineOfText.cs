using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Scenes.Game
{
    /// <summary>
    /// A class representing a single-line text element, such as a message or label.
    /// </summary>
    public class LineOfText : Element
    {

        public string Id { get => _id; set => _id = value; }
        public Component Parent { get => _parent; set => _parent = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }

        public LineOfText(string id, Component parent, Vector2 position, string text, SpriteFont font, Color color) 
            : base(id, parent, position)
        {
            Id = id;
            Text = text;
            Font = font;
            Position = position;
            Color = color;
            Rectangle = CreateRectangle();
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
    }
}
