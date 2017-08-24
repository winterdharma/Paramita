using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Scenes
{
    public class Background : Element
    {
        private Point _size;
        public Texture2D Texture { get; set; }
        public Point Size
        {
            get { return _size; }
            set { _size = value; _rectangle = CreateRectangle(); }
        }

        public Background(string id, Component parent, Vector2 position, Texture2D texture, Point size) 
            : base(id, parent, position)
        {
            Texture = texture;
            Size = size;
            parent.RectangleUpdated += OnPanelRectangleUpdate;
        }

        private void OnPanelRectangleUpdate(object sender, RectangleEventArgs e)
        {
            _rectangle = e.Rectangle;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, _rectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, Size.X, Size.Y);
        }
    }
}
