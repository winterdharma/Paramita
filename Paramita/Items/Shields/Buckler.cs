using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Armors
{
    public class Buckler : Shield
    {
        private string description;


        public Buckler(Texture2D texture, Rectangle rect, string description) : base(texture, rect, 14, 0, 2, 0)
        {
            name = "Buckler";
            this.description = description;
            equipType = EquipType.Hand;
        }


        public override string ToString()
        {
            return name;
        }

        public override string GetDescription()
        {
            return name;
        }
    }
}
