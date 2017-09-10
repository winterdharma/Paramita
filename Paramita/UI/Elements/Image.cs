using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Base;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// Handles the content and display logic for a discrete image drawn to the screen.
    /// </summary>
    public class Image : Element
    {
        #region Constructors
        public Image(string id, Component parent, Vector2 position, Texture2D texture, 
            Color normal, Color highlighted, int drawOrder, float scale = 1) 
            : base(id, parent, position, normal, highlighted, drawOrder)
        {
            Id = id;
            Texture = texture;
            Visible = true;
            Enabled = true;
            Scale = scale;
            Rectangle = CreateRectangle();
        }
        #endregion

        #region Properties
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        #endregion

        #region Initialization
        protected override Rectangle CreateRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y,
                (int)(Texture.Bounds.Width * Scale), (int)(Texture.Bounds.Height * Scale));
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, Position, null, Color, 0f, new Vector2(0,0), 
                    Scale, SpriteEffects.None, 0f);
        }
    }
}
