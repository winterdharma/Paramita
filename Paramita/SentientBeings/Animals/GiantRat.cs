using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using System.Collections.Generic;

namespace Paramita.SentientBeings.Animals
{
    public class GiantRat : Animal
    {
        public GiantRat(Texture2D sprites, Rectangle rightFacing, Rectangle leftFacing) 
            : base(sprites, rightFacing, leftFacing)
        {
            hitPoints = 5;
            protection = 2;
            magicResistance = 5;
            strength = 3;
            attackSkill = 8;
            defenseSkill = 10;
            precision = 5;
            morale = 6;
            encumbrance = 1;
            fatigue = 0;
            size = 1;

            naturalWeapons = new List<Weapon>();

            attacks = new List<Weapon>();
        }
    }
}
