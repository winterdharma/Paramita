using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.UI.Scenes.Game
{
    public class SpriteElement
    {
        public string Label { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get; set; }
        public Color Color { get; set; }
    }
}
