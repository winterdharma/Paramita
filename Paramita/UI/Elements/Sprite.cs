using Paramita.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Elements
{
    public class Sprite : Element
    {
        public Sprite(string id, Component parent, Vector2 position) : base(id, parent, position)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override Rectangle CreateRectangle()
        {
            throw new NotImplementedException();
        }
    }
}
