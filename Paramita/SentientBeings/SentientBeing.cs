using Microsoft.Xna.Framework;
using RogueSharp.DiceNotation;

namespace Paramita.SentientBeings
{
    /*
     * This is the base class for all models of sentient beings in the game.
     * Sentient beings include: Player, animals, monsters, non-player characters,
     * spirits, demons, gods, etc. Basically any entity with a mind and behavior
     * that interacts with non-sentient stuff in the game.
     * Bacteria, trees, and rocks are not sentient beings.
     * 
     * This is just a sketchy starting point of variables for all beings.
     */ 
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
