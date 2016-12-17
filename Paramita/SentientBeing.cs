using Microsoft.Xna.Framework;
using RogueSharp.DiceNotation;

namespace Paramita
{
    public class SentientBeing : DrawableGameComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int AttackBonus { get; set; }
        public int ArmorClass { get; set; }
        public DiceExpression Damage { get; set; }
        public int Health { get; set; }
        public string Name { get; set; }

        public SentientBeing(GameController game) : base(game) { }
    }
}
