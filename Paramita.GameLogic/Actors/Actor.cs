using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;
using System.Linq;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Actions;

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
        public Compass Direction { get; }
        public Point Origin { get; }
        public Point Destination { get; }
         
        public MoveEventArgs(Compass direction, Point tileOrigin, Point tileDest)
        {
            Direction = direction;
            Origin = tileOrigin;
            Destination = tileDest;
        }
    }



    public abstract class Actor : IEquatable<Actor>
    {
        #region Fields
        protected string name;

        protected Tile _currentTile;

        private Inventory _inventory;

        protected int _hitPoints;
        protected int _protection;
        protected int _shieldProtection;
        protected int _magicResistance;
        protected int _strength;
        protected int _morale;
        protected int _attackSkill;
        protected int _defenseSkill;
        protected int _parry;
        protected int _precision;
        protected int _encumbrance;
        protected int _fatigue;
        protected int _size;
        protected int _timesAttacked;

        // status related flags
        protected bool _isDead = false;
        #endregion


        #region Events
        public event EventHandler<InventoryChangeEventArgs> OnInventoryChange;
        public event EventHandler<MoveEventArgs> OnMoveAttempt;
        public event EventHandler<StatusMessageEventArgs> OnStatusMsgSent;
        public event EventHandler<MoveEventArgs> OnActorDeath;
        #endregion


        public Actor(BeingType beingType)
        {
            BeingType = beingType;
            Facing = Compass.East;
            Inventory = new Inventory();
        }


        #region Properties
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

        public Inventory Inventory
        {
            get { return _inventory; }
            protected set { _inventory = value; }
        }

        public List<Weapon> Attacks { get { return Inventory.Weapons; } }

        public List<Shield> Shields { get { return Inventory.Shields; } }

        public int HitPoints { get { return _hitPoints; } }

        public int Protection { get { return _protection; } }

        public int ShieldProtection
        {
            get
            {
                int protection = 0;
                for (int x = 0; x < Inventory.Shields.Count; x++)
                {
                    protection += Inventory.Shields[x].Protection;
                }
                return protection;
            }
        }

        public int Strength { get { return _strength; } }

        public int Morale { get { return _morale; } }

        public int AttackSkill { get { return _attackSkill; } }

        public int DefenseSkill
        {
            get
            {
                int weaponsMods = 0;
                for (int x = 0; x < Inventory.Weapons.Count; x++)
                {
                    weaponsMods += Inventory.Weapons[x].DefenseModifier;
                }
                return _defenseSkill + weaponsMods;
            }
        }

        public int Parry
        {
            get
            {
                int shieldParry = 0;
                for (int x = 0; x < Inventory.Shields.Count; x++)
                {
                    shieldParry += Inventory.Shields[x].Parry;
                }
                return shieldParry;
            }
        }

        public int Fatigue { get { return _fatigue; } }

        public int FatigueAttPenalty
        {
            get { return _fatigue / 20; }
        }

        public int FatigueDefPenalty
        {
            get { return _fatigue / 10; }
        }

        public int FatigueCriticalPenalty
        {
            get { return _fatigue / 15; }
        }

        public int Size { get { return _size; } }

        public int TimesAttacked
        {
            get { return _timesAttacked; }
            set { _timesAttacked = value; }
        }

        public bool IsDead { get { return _isDead; } }
        #endregion


        #region Abstract Methods
        // helper methods for child class constructors
        protected abstract void InitializeAttributes();
        protected abstract void InitializeInventory();
        // This method is the verbose report on a sentient being
        public abstract string GetDescription();
        #endregion


        #region Combat Related API
        // conduct all of a being's attacks for the turn
        public void Attack(Actor defender)
        {
            Attack attack;
            //GameScene.PostNewStatus(this + " attacked " + defender + ".");
            for(int x = 0; x < Inventory.Weapons.Count; x++)
            {
                attack = new Attack(this, Inventory.Weapons[x], defender);
                OnStatusMsgSent?.Invoke(this, new StatusMessageEventArgs(attack.AttackReport));
                if (IsDead || defender.IsDead)
                {
                    break;
                }
            }
        }


        // used to check for a RepelMeeleeAttack attempt
        public Weapon GetLongestWeapon()
        {
            if (Attacks.Count == 0)
                return null;

            Weapon longestWeapon = Inventory.Weapons[0];

            if (Attacks.Count > 1)
            {
                for (int x = 1; x < Attacks.Count; x++)
                {
                    if (Attacks[x].Length > longestWeapon.Length)
                        longestWeapon = Attacks[x];
                }
            }
            return longestWeapon;
        }


        public void TakeDamage(int damage)
        {
            if(damage > 0)
            {
                _hitPoints -= damage;
                CheckForDeath();
            }
        }


        public void IncrementTimesAttacked()
        {
            _timesAttacked++;
        }


        public void AddEncumbranceToFatigue()
        {
            int totalEncumbrance = GetTotalEncumbrance();

            _fatigue += totalEncumbrance;
        }
        #endregion

        
        #region Inventory Related API
        public bool Acquire(Item item)
        {
            if (!Inventory.AddItemToEquipment(item))
                return Inventory.AddItemToStorage(item);
            else
                return true;
        }

        public bool Discard(Item item)
        {
            if (Remove(item))
            {
                CurrentTile.AddItem(item);
                return true;
            }
            else
                return false;
        }
        #endregion


        public bool Equals(Actor other)
        {
            if (this.GetType() == other.GetType())
                return true;
            return false;
        }

        public virtual void Update()
        {
            if(_fatigue > 0)
                _fatigue--;
            CheckForDeath();
        }


        // the default string equivalent for this being
        public override string ToString()
        {
            return name;
        }


        #region Protected Methods
        protected virtual bool MoveTo(Compass direction)
        {
            if (direction == Compass.None)
                return false;

            Facing = direction;
            Tile currentTile = CurrentTile;
            // check to see if the bool check for tile change works as expected
            OnMoveAttempt?.Invoke(this, new MoveEventArgs(direction, Point.Zero, Point.Zero));

            Tile newTile = CurrentTile;
            if (newTile == currentTile)
                return false;
            else
                return true;
        }

        protected int GetTotalEncumbrance()
        {
            int total = _encumbrance;

            for (int i = 0; i < Inventory.Equipment.Length; i++)
            {
                total += GetItemEncumbrance(Inventory.Equipment[i]);
            }

            return total;
        }
        #endregion


        #region Helper Methods
        protected void SubscribeToEvents()
        {
            Inventory.OnInventoryChange += HandleInventoryChange;
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChange?.Invoke(this, eventArgs);
        }

        private void CheckForDeath()
        {
            if (_hitPoints < 1)
            {
                _isDead = true;
                OnActorDeath?.Invoke(this, new MoveEventArgs(Compass.None, CurrentTile.TilePoint, Point.Zero));
            }
                
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
        
        private bool Remove(Item item)
        {
            if (!Inventory.RemoveItemFromEquipment(item))
                return Inventory.RemoveItemFromStorage(item);
            else
                return true;
        }
        #endregion
    }
}
