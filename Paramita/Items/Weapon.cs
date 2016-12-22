﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items
{
    /*
     * Weapons are used to engage in combat or otherwise harm living things.
     * This is a base class not intended to be directly instantiated.
     */
    public abstract class Weapon : Item
    {
        public Weapon(GameController game, Texture2D texture, Rectangle textureRect, string name, string description) 
            : base(game, texture, textureRect, name, description)
        {
        }
    }
}