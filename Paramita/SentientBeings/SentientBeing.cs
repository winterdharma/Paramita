using Microsoft.Xna.Framework;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;
using RogueSharp.DiceNotation;

namespace Paramita.SentientBeings
{
    public enum HumanoidBodyParts
    {
        Head,
        Torso,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }


    /*
     * This is the base class for all models of sentient beings in the game.
     * Sentient beings include: Player, animals, monsters, non-player characters,
     * spirits, demons, gods, etc. Basically any entity with a mind and behavior
     * that interacts with non-sentient stuff in the game.
     * Bacteria, trees, and rocks are not sentient beings.
     * 
     * This is just a sketchy starting point of variables for all beings.
     */ 
    public class SentientBeing
    {
        // location on the tilemap
        protected Tile currentTile;

        // the direction a being is facing
        protected Compass facing;

        // items in a being's possession
        protected Item[] items;

        // equiped items
        protected Item leftHand;
        protected Item rightHand;
        protected Item head;
        protected Item body;
        protected Item feet;
        protected Item extra1;
        protected Item extra2;

        // combat related attributes
        protected int hitPoints;
        protected int protection;
        protected int magicResistance;
        protected int strength;
        protected int morale;
        protected int attackSkill;
        protected int defenseSkill;
        protected int precision;
        protected int encumbrance;
        protected int fatigue;


        public int AttackSkill { get { return attackSkill; } }

        public int DefenseSkill { get { return defenseSkill; } }

        public int Fatigue { get { return fatigue; } }

        public int Strength { get { return strength; } }

        public int Protection { get { return protection; } }

        public Compass Facing { get; protected set; }

        public SentientBeing(GameController game)
        { }
    }
}
