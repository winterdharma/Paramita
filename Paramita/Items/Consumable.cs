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
        public Consumable(GameController game, Texture2D texture, string name, string description) 
            : base(game, texture, name, description)
        {
        }
    }
}
