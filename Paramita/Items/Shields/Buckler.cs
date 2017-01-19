using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Armors
{
    public class Buckler : Shield
    {
        private string description;


        public Buckler(Texture2D texture) : base(texture, 14, 0, 2, 0)
        {
            name = "Buckler";
            this.description = "A small shield for parrying attacks.";
            equipType = EquipType.Hand;
        }


        public override string ToString()
        {
            return name;
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}
