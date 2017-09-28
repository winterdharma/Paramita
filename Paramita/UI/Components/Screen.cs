using Paramita.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Paramita.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Components
{
    /// <summary>
    /// The Screen component is intended for simple static Scenes that need only a background image
    /// with one or more Elements drawn onto it. The parent Scene should provide the Elements list
    /// after the Screen component is constructed.
    /// </summary>
    public class Screen : Component
    {
        #region Fields
        private const string BACKGROUND_ID = "background";
        
        #endregion

        #region Constructors
        public Screen(Scene parent, int drawOrder) : base(parent, drawOrder)
        {
            
        }
        #endregion

        #region Properties
        #endregion

        #region Initialization
        protected override Rectangle UpdatePanelRectangle()
        {
            return _parentRectangle;
        }
        #endregion

        #region Public API
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}
