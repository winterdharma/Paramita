using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Scenes.Game
{
    public class TextElement
    {
        public string Label { get; set; }
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public TextElement() { }

        public TextElement(string label, string text, SpriteFont font, Vector2 position, Color color)
        {
            Label = label;
            Text = text;
            Font = font;
            Position = position;
            Color = color;
        }
    }
}
