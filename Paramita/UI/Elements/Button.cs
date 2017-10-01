using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Base;

namespace Paramita.UI.Elements
{
    /// <summary>
    /// The Button element is simply an Image element with a centered LineOfText element drawn over its Texture.
    /// </summary>
    public class Button : Image
    {
        #region Constructor
        public Button(string id, Component parent, Point position, Texture2D texture, 
            LineOfText label, Color normal, Color highlighted, int drawOrder, float scale = 1f) 
            : base(id, parent, position, texture, normal, highlighted, drawOrder, scale)
        {
            Label = label;
            Label.Position = Label.GetCenteredPosition(this.Rectangle);
        }
        #endregion

        #region Property
        public LineOfText Label { get; set; }
        #endregion

        #region Public API
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            Label.Draw(gameTime, spriteBatch);
        }

        public override void Show()
        {
            base.Show();
            Label.Show();
        }

        public override void Hide()
        {
            base.Hide();
            Label.Hide();
        }

        public override void Highlight()
        {
            Label.Highlight();
        }

        public override void Unhighlight()
        {
            Label.Unhighlight();
        }
        #endregion
    }
}
