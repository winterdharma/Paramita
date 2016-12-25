using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Armors
{
    public class Shield : Armor
    {
        private string name;
        private string description;


        public Shield(Texture2D texture, Rectangle rect, string description) : base(texture, rect)
        {
            name = "Shield";
            this.description = description;
        }


        public override string ToString()
        {
            return name;
        }
    }
}
