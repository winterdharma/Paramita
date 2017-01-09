using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items
{
    /*
     * Armors are items used to protect beings from harm, usually from combat damage.
     * Includes protection from magic or the elements as well.
     * 
     * This is an abstract class not intended to be instantiated.
     */
    public abstract class Armor : Item
    {

        private int encumbrance;

        public int Encumbrance { get { return encumbrance; } }

        public Armor(Texture2D texture, Rectangle rect) 
            : base(texture, rect)
        {
        }
    }
}
