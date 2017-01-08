using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Consumables
{
    public class Meat : Consumable
    {
        private string description;
        private int sustenance;

        public int Sustanence
        {
            get { return sustenance; }
        }

        public Meat(Texture2D texture, Rectangle rect, string description) : base(texture, rect)
        {
            name = "Salted Meat";
            this.description = description;
            sustenance = 480;
        }



        public override string ToString()
        {
            return "a chunk of " + name;
        }

        public override string GetDescription()
        {
            return "a chunk of " + name;
        }
    }
}
