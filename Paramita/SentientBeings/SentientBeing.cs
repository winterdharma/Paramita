using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;
using RogueSharp.DiceNotation;
using System.Collections.Generic;

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
    public abstract class SentientBeing
    {
        // the string returned by ToString()
        protected string name;

        // reference to the GameScene for access to GameScene.Map
        protected GameScene gameScene;
        protected Camera camera;
        protected CombatManager combatManager;

        // location on the tilemap
        protected Tile currentTile;

        // a vector2 position on the tilemap expressed in pixels
        protected Vector2 position;

        // the direction a being is facing
        protected Compass facing;

        // sprite sheet for images
        protected Texture2D spritesheet;

        // left and right facing source rectangles
        protected Rectangle rightFacing;
        protected Rectangle leftFacing;

        // items in a being's possession
        protected Item[] items;

        // a list of the natural weapons a being possesses (fist, bite, claw, etc)
        // *  these define a being's default attack without equiped weapon items
        // *  some beings (like animals) only use these natural weapons
        // *  for other beings, equiped items will replace these
        protected List<Weapon> naturalWeapons;

        // a list of the weapons a being attacks with (could be > 1 or 0)
        protected List<Weapon> attacks;

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
        protected int size;

        // status related flags
        protected bool isDead = false;

        public bool IsDead { get { return isDead; } }

        public Tile CurrentTile
        {
            get { return currentTile; }
            set
            {
                currentTile = value;
                position = gameScene.Map.GetTilePosition(currentTile);
            } 
        }

        public Rectangle CurrentSprite { get; set; }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int HitPoints { get { return hitPoints; } }

        public int Morale { get { return morale; } }

        public int AttackSkill { get { return attackSkill; } }

        public int DefenseSkill
        {
            get
            {
                int weaponsMods = 0;
                for(int x = 0; x < attacks.Count; x++)
                {
                    weaponsMods += attacks[x].DefenseModifier;
                }
                return defenseSkill + weaponsMods;
            }
        }

        public int Fatigue { get { return fatigue; } }

        public int FatigueAttPenalty
        {
            get { return fatigue / 20; }
        }
        public int FatigueDefPenalty
        {
            get { return fatigue / 10; }
        }

        public int Strength { get { return strength; } }

        public int Protection { get { return protection; } }

        public List<Weapon> Attacks { get { return attacks; } }

        public Compass Facing
        {
            get { return facing; }
            protected set { facing = value; }
        }

        public int Size { get { return size; } }




        public SentientBeing(GameScene gameScene, Texture2D sprites, Rectangle right, Rectangle left)
        {
            this.gameScene = gameScene;
            camera = gameScene.Camera;
            combatManager = gameScene.CombatManager;

            spritesheet = sprites;
            rightFacing = right;
            leftFacing = left;

            CurrentSprite = rightFacing;
            
        }




        protected virtual bool MoveTo(Compass direction)
        {
            // exit if no direction was given
            if(direction == Compass.None)
            {
                return false;
            }

            facing = direction;
            SetCurrentSprite();
            Tile newTile = gameScene.Map.GetTile(CurrentTile.TilePoint + Direction.GetPoint(Facing));

            // check to see if:
            //     * newTile is not outside the TileMap
            //     * newTile is walkable
            if (newTile != null && newTile.IsWalkable == true)
            {
                CurrentTile = newTile;
                return true;
            }
            return false;
        }



        // conduct all of a being's attacks for the turn
        public void Attack(SentientBeing defender)
        {
            gameScene.PostNewStatus(this + " attacked " + defender + ".");
            for(int x = 0; x < attacks.Count; x++)
            {
                combatManager.ResolveAttack(this, attacks[x], defender);
                if (this.IsDead== true || defender.IsDead == true)
                    break;
            }
        }



        // Applies damage to the being's hitPoints
        // Validates that damage is greater than zero before applying damage
        public void TakeDamage(int damage)
        {
            if(damage > 0)
            {
                hitPoints -= damage;
                CheckForDeath();
            }
        }



        public virtual void Update(GameTime gameTime)
        {
            CheckForDeath();
        }


        // Checks being's @hitPoints and sets @isDead to true if zero or less
        private void CheckForDeath()
        {
            if (hitPoints < 1)
                isDead = true;
        }



        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null,
                camera.Transformation);

            spriteBatch.Draw(
                spritesheet,
                position,
                CurrentSprite,
                Color.White
                );

            spriteBatch.End();
        }



        /* 
        * Sets CurrentSprite to something appropriate for the Compass direction passed to it
        * Currently, the player animations are in four directions, but movement can be
        * in eight directions.
        */
        protected void SetCurrentSprite()
        {
            // set the animation according to the new facing
            if (facing == Compass.North)
                CurrentSprite = rightFacing;
            else if (Facing == Compass.South)
                CurrentSprite = leftFacing;
            else if (Facing == Compass.Northeast || Facing == Compass.East || Facing == Compass.Southeast)
                CurrentSprite = rightFacing;
            else if (Facing == Compass.Northwest || Facing == Compass.West || Facing == Compass.Southwest)
                CurrentSprite = leftFacing;
        }



        // helper method for CombatManager that returns the weapon in the being's
        // Attacks list that is the longest one (used for resolving defender repel attacks)
        // If there are > 1 weapon with the longest length, returns the first one that appears
        // in the Attacks list
        public Weapon GetLongestWeapon()
        {
            Weapon longestWeapon = attacks[0];

            if(attacks.Count > 1)
            {
                for (int x = 1; x < attacks.Count; x++)
                {
                    if (attacks[x].Length > longestWeapon.Length)
                        longestWeapon = attacks[x];
                }
            }

            return longestWeapon;
        }



        // the default string equivalent for this being
        public override string ToString()
        {
            return name;
        }



        // This method is the verbose report on a sentient being
        public abstract string GetDescription();


        // being types should implement this method to handle equipping items
        public abstract bool EquipItem(Item item);
        
        
        // beings should implement this method to initialize and update their Attacks list
        public abstract void UpdateAttacks();
    }
}
