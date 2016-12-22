using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items
{
    /*
     * Consumables are items that are destroyed when used and converted
     * to some effect. Example: A healing potion is drunk and yeilds hit points
     * to the consumer. The potion is destroyed when used.
     * 
     * This is an abstract class not intended to be instantiated.
     */
    public abstract class Consumable : Item
    {
        public Consumable(Texture2D texture, Rectangle rect, string name, string description) 
            : base(texture, rect)
        {
        }
    }
}
