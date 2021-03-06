﻿namespace Paramita.GameLogic.Items
{
    /*
     * Valuables are items whose primary use is as currency or as a store
     * of wealth. Example: Cash, jewelry, gemstones, precious metals.
     * 
     * This is an abstract class not intended to be instantiated.
     */
    public abstract class Valuable : Item
    {
        public Valuable(ItemType type, string name) : base(type, name)
        {
        }
    }
}
