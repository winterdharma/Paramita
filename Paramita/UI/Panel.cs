using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI
{
    public struct Panel
    {
        public Color backColor;
        public Color color;
        public int character;

        public Panel(Color backColor, Color color, int character)
        {
            this.backColor = backColor;
            this.color = color;
            this.character = character;
        }

        public override string ToString()
        {
            return string.Format("C:{0}, B:{1}, F:{2}", character, backColor, color);
        }
    }
}
