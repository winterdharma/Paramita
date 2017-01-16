using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;
using RogueSharp.DiceNotation;
using System.Collections.Generic;
using System;
using System.Linq;

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

        // used to determine SentientBeing's placement on a TileMap
        protected Tile currentTile;
        protected Vector2 position;

        // used to display SentientBEing's sprite in UI
        protected Compass facing;
        protected Texture2D spritesheet;
        protected Rectangle rightFacing;
        protected Rectangle leftFacing;

        // items in a being's possession - how these arrays are implemented 
        // is defined by child classes of SentientBeing
        protected Item[] unequipedItems;
        protected Item[] equipedItems;

        // a list of the natural weapons a being possesses (fist, bite, claw, etc)
        // *  these define a being's default attack without equiped weapon items
        // *  some beings (like animals) only use these natural weapons
        // *  for other beings, equiped items will replace these
        protected List<Weapon> naturalWeapons;

        // a list of the weapons a being attacks with (could be > 1 or 0)
        protected List<Weapon> attacks;
        protected List<Shield> shields;

        // combat related attributes
        protected int hitPoints;
        protected int protection;
        protected int shieldProtection;
        protected int magicResistance;
        protected int strength;
        protected int morale;
        protected int attackSkill;
        protected int defenseSkill;
        protected int parry;
        protected int precision;
        protected int encumbrance;
        protected int fatigue;
        protected int size;
        protected int timesAttacked;

        // status related flags
        protected bool isDead = false;

        
        // public accessors
        public Tile CurrentTile
        {
            get { return currentTile; }
            set
            {
                currentTile = value;
                position = gameScene.Map.GetTilePosition(currentTile);
            } 
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Compass Facing
        {
            get { return facing; }
            protected set { facing = value; }
        }
        public Rectangle CurrentSprite { get; set; }
        public List<Weapon> Attacks { get { return attacks; } }
        public List<Shield> Shields { get { return shields; } }
        public int HitPoints { get { return hitPoints; } }
        public int Protection { get { return protection; } }
        public int ShieldProtection
        {
            get
            {
                int protection = 0;
                for(int x = 0; x<shields.Count; x++)
                {
                    protection += shields[x].Protection;
                }
                return protection;
            }
        }
        public int Strength { get { return strength; } }
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
        public int Parry
        {
            get
            {
                int shieldParry = 0;
                for(int x = 0; x < shields.Count; x++)
                {
                    shieldParry += shields[x].Parry;
                }
                return shieldParry;
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
        public int FatigueCriticalPenalty
        {
            get { return fatigue / 15; }
        }
        public int Size { get { return size; } }
        public int TimesAttacked
        {
            get
            {
                return timesAttacked;
            }
            set
            {
                timesAttacked = value;
            }
        }
        public bool IsDead { get { return isDead; } }




        public SentientBeing(GameScene gameScene, Texture2D sprites, Rectangle right, Rectangle left)
        {
            this.gameScene = gameScene;
            camera = gameScene.Camera;

            spritesheet = sprites;
            rightFacing = right;
            leftFacing = left;

            CurrentSprite = rightFacing;
        }


        // helper methods for child class constructors
        protected abstract void InitializeAttributes();
        protected abstract void InitializeItemLists();


        
        public bool TryToEquipItem(Item item)
        {
            bool isEquiped = false;

            if (EquipNaturalWeapon(item) == true)
            {
                isEquiped = true;
            }
            else if (EquipWeapon(item) == true)
            {
                isEquiped = true;
            }
            else if (EquipItem(item) == true)
            {
                isEquiped = true;
            }

            if (isEquiped == true)
            {
                UpdateAttacks();
                UpdateShields();
            }

            return isEquiped;
        }


        protected bool EquipNaturalWeapon(Item item)
        {
            if (IsValidNaturalWeaponToEquip(item) == false)
                return false;
            var locations = GetLocationForEquipType(item.EquipType);
            if (IsWeaponEquipedAtLocations(locations) == true)
                return false;
            if (TryToEquipItemAtLocations(item, locations) == true)
                return true;

            return false;
        }


        protected bool EquipWeapon(Item item)
        {
            if ((item is Weapon) == false)
                return false;
            var locations = GetLocationForEquipType(item.EquipType);
            if (TryToReplaceNaturalWeaponAtLocations(item, locations) == true)
                return true;
            if (TryToEquipItemAtLocations(item, locations) == true)
                return true;

            return false;
        }


        private bool TryToReplaceNaturalWeaponAtLocations(Item item, List<int> locations)
        {
            for (int x = 0; x < locations.Count; x++)
            {
                int location = locations[x];
                if (IsNaturalWeaponEquiped(location) == true)
                {
                    equipedItems[location] = item;
                    return true;
                }
            }
            return false;
        }

        protected bool IsValidNaturalWeaponToEquip(Item item)
        {
            if (!(item is NaturalWeapon) || item.EquipType == EquipType.None)
                return false;
            return true;
        }


        protected bool IsWeaponEquipedAtLocations(List<int> locations)
        {
            for (int x = 0; x < locations.Count; x++)
            {
                int location = locations[x];
                if (IsWeaponEquiped(location) == true)
                    return true;
            }
            return false;
        }

        private bool IsNaturalWeaponEquiped(int location)
        {
            if ((equipedItems[location] is NaturalWeapon) == true)
                return true;
            return false;
        }

        protected abstract List<int> GetLocationForEquipType(EquipType type);


        protected bool TryToEquipItemAtLocations(Item item, List<int> locations)
        {
            for (int x = 0; x < locations.Count; x++)
            {
                int location = locations[x];
                if (IsEquipLocationEmpty(location) == true)
                {
                    EquipItemLocation(item, location);
                    return true;
                }
            }
            return false;
        }


        protected bool IsWeaponEquiped(int location)
        {
            if (equipedItems[location] is Weapon)
                return true;
            return false;
        }


        protected bool IsEquipLocationEmpty(int location)
        {
            if (equipedItems[location] == null)
                return true;
            return false;
        }


        protected bool EquipItemLocation(Item item, int location)
        {
            if (equipedItems[location] != null)
                return false;

            equipedItems[location] = item;
            return true;
        }


        protected bool EquipItem(Item item)
        {
            var locations = GetLocationForEquipType(item.EquipType);
            if (TryToEquipItemAtLocations(item, locations) == true)
                return true;
            return false;
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
            Attack attack;
            GameScene.PostNewStatus(this + " attacked " + defender + ".");
            for(int x = 0; x < attacks.Count; x++)
            {
                attack = new Attack(this, attacks[x], defender);
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


        public void IncrementTimesAttacked()
        {
            timesAttacked++;
        }


        public void AddEncumbranceToFatigue()
        {
            int totalEncumbrance = GetTotalEncumbrance();

            fatigue += totalEncumbrance;
        }



        public virtual void Update(GameTime gameTime)
        {
            if(fatigue > 0)
                fatigue--;
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



        // Checks the being's equip items for Weapons or NaturalWeapons
        public void UpdateAttacks()
        {
            // reset the attacks list
            attacks.Clear();

            // check for bonus attack NaturalWeapons
            for (int x = 0; x < naturalWeapons.Count; x++)
            {
                if (naturalWeapons[x].EquipType == EquipType.None)
                {
                    attacks.Add(naturalWeapons[x]);
                }
            }

            for (int x = 0; x < equipedItems.Length; x++)
            {
                if (equipedItems[x] is Weapon)
                {
                    attacks.Add(equipedItems[x] as Weapon);
                }
            }

            attacks = attacks.OrderBy(w => w.Length).ToList();
            Console.WriteLine(attacks.ToString());
        }



        public void UpdateShields()
        {
            shields.Clear();

            for (int x = 0; x < equipedItems.Length; x++)
            {
                if (equipedItems[x] is Shield)
                {
                    shields.Add(equipedItems[x] as Shield);
                }
            }
        }

        protected int GetTotalEncumbrance()
        {
            int total = encumbrance;

            for(int x = 0; x < equipedItems.Length; x++)
            {
                total += GetItemEncumbrance(equipedItems[x]);
            }

            return total;
        }


        private int GetItemEncumbrance(Item item)
        {
            if (item is Armor)
            {
                var itemAsArmor = item as Armor;
                return itemAsArmor.Encumbrance;
            }
            else if (item is Shield)
            {
                var itemAsShield = item as Shield;
                return itemAsShield.Encumbrance;
            }
            return 0;
        }

    }
}
