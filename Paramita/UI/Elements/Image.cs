using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Base.Game
{
    public class Image : Element
    {
        public Image(string id, Component parent, Vector2 position, Texture2D texture, Color color) 
            : base(id, parent, position)
        {
            Id = id;
            Texture = texture;
        }

        public string Id { get => _id; set => _id = value; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Color Color { get => _color; set => _color = value; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color);
        }

        public override void Update(GameTime gameTime)
        {

        }

        protected override Rectangle CreateRectangle()
        {
            return Texture.Bounds;
        }
    }
}
