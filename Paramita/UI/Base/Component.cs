using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Scenes
{
    /// <summary>An collection of several elements that combine to form a single component of a 
    /// Scene.</summary>
    public abstract class Component
    {
        private List<Element> _elements = new List<Element>();

        public Component()
        {

        }

        public List<Element> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        public bool Enabled { get; internal set; }
        public bool Visible { get; internal set; }

        public virtual void Update(GameTime gameTime)
        {
            foreach(var element in _elements)
            {
                element.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }
    }
}
