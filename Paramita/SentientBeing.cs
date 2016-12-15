using Microsoft.Xna.Framework.Graphics;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita
{
    public class SentientBeing
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }

        public int AttackBonus { get; set; }
        public int ArmorClass { get; set; }
        public DiceExpression Damage { get; set; }
        public int Health { get; set; }
        public string Name { get; set; }
    }
}
