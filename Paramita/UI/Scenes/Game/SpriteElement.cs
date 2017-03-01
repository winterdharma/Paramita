using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Scenes.Game
{
    public class SpriteElement
    {
        public string Label { get; set; }
        public Texture2D Texture { set; get; }
        public Vector2 Position { set; get; }
        public Color Color { set; get; }
    }
}
