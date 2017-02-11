using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;
using System.Linq;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic.Actors
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

    public enum BeingType
    {
        GiantRat,
        HumanPlayer
    }

    public class MoveEventArgs : EventArgs
    {
        public Compass Direction { get; private set; }
        public Point TilePoint { get; private set; }
         
        public MoveEventArgs(Compass direction, Point tilePoint)
        {
            Direction = direction;
            TilePoint = tilePoint;
        }
    }


    public abstract class Actor
    {
        protected string name;

        protected Tile _currentTile;

        protected Item[] unequipedItems;
        protected Item[] equipedItems;

        protected List<Weapon> naturalWeapons;
        protected List<Weapon> attacks;
        protected List<Shield> shields;

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
        public BeingType BeingType { get; protected set; }

        public Tile CurrentTile
        {
            get { return _currentTile; }
            set
            {
                _currentTile = value;
            } 
        }
        
        public Compass Facing { get; private set; }

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
            get { return timesAttacked; }
            set { timesAttacked = value; }
        }
        public bool IsDead { get { return isDead; } }

        public Item[] EquipmentSlots { get { return equipedItems; } }

        public event EventHandler<MoveEventArgs> OnMoveAttempt;

        public Actor(BeingType beingType)
        {
            BeingType = beingType;
            Facing = Compass.East;
        }



        // helper methods for child class constructors
        protected abstract void InitializeAttributes();
        protected abstract void InitializeItemLists();
        public abstract List<int> GetLocationForEquipType(EquipType type);


        public bool TryToEquipItem(Item item)
        {
            return new EquipItem(this, item).IsEquipped;
        }
        

        public bool IsNaturalWeaponEquipedAt(int location)
        {
            if ((equipedItems[location] is NaturalWeapon) == true)
                return true;
            return false;
        }

        
        public bool IsWeaponEquippedAt(int location)
        {
            if (equipedItems[location] is Weapon)
                return true;
            return false;
        }


        public bool IsEquipmentSlotEmpty(int location)
        {
            if (EquipmentSlots[location] == null)
                return true;
            return false;
        }


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


        protected virtual bool MoveTo(Compass direction)
        {
            if(direction == Compass.None)
                return false;

            Facing = direction;
            Tile currentTile = CurrentTile;
            // check to see if the bool check for tile change works as expected
            OnMoveAttempt?.Invoke(this, new MoveEventArgs(direction, Point.Zero));

            Tile newTile = CurrentTile;
            if (newTile == currentTile)
                return false;
            else
                return true;
        }


        // conduct all of a being's attacks for the turn
        public void Attack(Actor defender)
        {
            Attack attack;
            //GameScene.PostNewStatus(this + " attacked " + defender + ".");
            for(int x = 0; x < attacks.Count; x++)
            {
                attack = new Attack(this, attacks[x], defender);
                if ( this.IsDead || defender.IsDead)
                    break;
            }
        }


        // used to check for a RepelMeeleeAttack attempt
        public Weapon GetLongestWeapon()
        {
            Weapon longestWeapon = attacks[0];

            if (attacks.Count > 1)
            {
                for (int x = 1; x < attacks.Count; x++)
                {
                    if (attacks[x].Length > longestWeapon.Length)
                        longestWeapon = attacks[x];
                }
            }
            return longestWeapon;
        }


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


        protected int GetTotalEncumbrance()
        {
            int total = encumbrance;

            for (int i = 0; i < equipedItems.Length; i++)
            {
                total += GetItemEncumbrance(equipedItems[i]);
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

        public virtual void Update()
        {
            if(fatigue > 0)
                fatigue--;
            CheckForDeath();
            //sprite.Update(gameTime);
        }


        private void CheckForDeath()
        {
            if (hitPoints < 1)
                isDead = true;
        }


        // the default string equivalent for this being
        public override string ToString()
        {
            return name;
        }


        // This method is the verbose report on a sentient being
        public abstract string GetDescription();
    }
}
