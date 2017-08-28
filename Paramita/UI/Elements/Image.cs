using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Base.Game
{
    /// <summary>
    /// Handles the content and display logic for a discrete image drawn to the screen.
    /// </summary>
    public class Image : Element
    {
        public Image(string id, Component parent, Vector2 position, Texture2D texture, Color color) 
            : base(id, parent, position)
        {
            Id = id;
            Texture = texture;
            Visible = true;
            Enabled = true;
            Color = color;
        }

        public string Id { get => _id; set => _id = value; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Color Color { get => _color; set => _color = value; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Visible)
                spriteBatch.Draw(Texture, Position, Color);
        }

        public override void Update(GameTime gameTime)
        {
            if(Enabled) { }
        }

        protected override Rectangle CreateRectangle()
        {
            return Texture.Bounds;
        }
    }
}
