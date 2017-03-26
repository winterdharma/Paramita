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

        protected Item[] _unequippedItems;
        protected Item[] _equippedItems;

        protected List<Weapon> _naturalWeapons;
        protected List<Weapon> _attacks;
        protected List<Shield> _shields;

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
        public event EventHandler<MoveEventArgs> OnMoveAttempt;
        public event EventHandler<StatusMessageEventArgs> OnStatusMsgSent;
        public event EventHandler<MoveEventArgs> OnActorDeath;
        #endregion


        public Actor(BeingType beingType)
        {
            BeingType = beingType;
            Facing = Compass.East;
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

        public List<Weapon> Attacks { get { return _attacks; } }

        public List<Shield> Shields { get { return _shields; } }

        public int HitPoints { get { return _hitPoints; } }

        public int Protection { get { return _protection; } }

        public int ShieldProtection
        {
            get
            {
                int protection = 0;
                for (int x = 0; x < _shields.Count; x++)
                {
                    protection += _shields[x].Protection;
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
                for (int x = 0; x < _attacks.Count; x++)
                {
                    weaponsMods += _attacks[x].DefenseModifier;
                }
                return _defenseSkill + weaponsMods;
            }
        }

        public int Parry
        {
            get
            {
                int shieldParry = 0;
                for (int x = 0; x < _shields.Count; x++)
                {
                    shieldParry += _shields[x].Parry;
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

        public Item[] EquipmentSlots
        {
            get { return _equippedItems; }
            protected set
            {
                _equippedItems = value;
                UpdateAttacks();
                UpdateShields();
            }
        }

        public Item[] StorageSlots { get { return _unequippedItems; } }
        #endregion


        #region Abstract Methods
        // helper methods for child class constructors
        protected abstract void InitializeAttributes();
        protected abstract void InitializeItemLists();
        public abstract List<int> GetLocationForEquipType(EquipType type);
        // This method is the verbose report on a sentient being
        public abstract string GetDescription();
        #endregion


        #region Item Related API
        public bool AcquireItem(Item item)
        {
            if (!AddItemToEquipment(item))
                return AddItemToStorage(item);
            else
                return true;
        }

        public bool DiscardItem(Item item)
        {
            if (!RemoveItemFromEquipment(item))
                return RemoveItemFromStorage(item);
            else
                return true;
        }

        public bool EquipItem(Item item)
        {
            if (RemoveItemFromStorage(item))
                return AddItemToEquipment(item);
            else
                return false;
        }

        public bool UnequipItem(Item item)
        {
            if (RemoveItemFromEquipment(item))
                return AddItemToStorage(item);
            else
                return false;
        }

        public void UseItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void ConsumeItem(Item item)
        {
            throw new NotImplementedException();
        }

        public bool IsNaturalWeaponEquipedAt(int location)
        {
            if ((_equippedItems[location] is NaturalWeapon) == true)
                return true;
            return false;
        }

        
        public bool IsWeaponEquippedAt(int location)
        {
            if (_equippedItems[location] is Weapon)
                return true;
            return false;
        }

        public bool IsEquipmentSlotEmpty(int location)
        {
            if (EquipmentSlots[location] == null)
                return true;
            return false;
        }
        #endregion


        #region Combat Related API
        // Checks the being's equip items for Weapons or NaturalWeapons
        public void UpdateAttacks()
        {
            // reset the attacks list
            _attacks.Clear();

            // check for bonus attack NaturalWeapons
            for (int x = 0; x < _naturalWeapons.Count; x++)
            {
                if (_naturalWeapons[x].EquipType == EquipType.None)
                {
                    _attacks.Add(_naturalWeapons[x]);
                }
            }

            for (int x = 0; x < _equippedItems.Length; x++)
            {
                if (_equippedItems[x] is Weapon)
                {
                    _attacks.Add(_equippedItems[x] as Weapon);
                }
            }

            _attacks = _attacks.OrderBy(w => w.Length).ToList();
            Console.WriteLine(_attacks.ToString());
        }


        public void UpdateShields()
        {
            _shields.Clear();

            for (int x = 0; x < _equippedItems.Length; x++)
            {
                if (_equippedItems[x] is Shield)
                {
                    _shields.Add(_equippedItems[x] as Shield);
                }
            }
        }


        // conduct all of a being's attacks for the turn
        public void Attack(Actor defender)
        {
            Attack attack;
            //GameScene.PostNewStatus(this + " attacked " + defender + ".");
            for(int x = 0; x < _attacks.Count; x++)
            {
                attack = new Attack(this, _attacks[x], defender);
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

            Weapon longestWeapon = _attacks[0];

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

            for (int i = 0; i < _equippedItems.Length; i++)
            {
                total += GetItemEncumbrance(_equippedItems[i]);
            }

            return total;
        }
        #endregion


        #region Helper Methods
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

        private bool AddItemToStorage(Item item)
        {
            var emptyAt = new List<int>();
            if (FindEmptySlots(StorageSlots, out emptyAt))
            {
                StorageSlots[emptyAt[0]] = item;
                return true;
            }
            else
                return false;
        }

        private bool RemoveItemFromStorage(Item item)
        {
            int foundAt;
            if (FindItemInSlots(StorageSlots, item, out foundAt))
            {
                StorageSlots[foundAt] = null;
                return true;
            }
            else
                return false;
        }

        private bool AddItemToEquipment(Item item)
        {
            if (item.EquipType == EquipType.None)
                return false;

            var validAt = GetLocationForEquipType(item.EquipType);

            foreach(var index in validAt)
            {
                if((item is Weapon || item is Shield) && !(item is NaturalWeapon) 
                    && EquipmentSlots[index] is NaturalWeapon)
                {
                    _equippedItems[index] = item;
                    EquipmentSlots = _equippedItems;
                    return true;
                }


                if(EquipmentSlots[index] == null)
                {
                    _equippedItems[index] = item;
                    EquipmentSlots = _equippedItems;
                    return true;
                }
            }
            return false;
        }

        private bool RemoveItemFromEquipment(Item item)
        {

            int foundAt;
            if(FindItemInSlots(EquipmentSlots, item, out foundAt))
            {
                _equippedItems[foundAt] = null;
                EquipmentSlots = _equippedItems;

                if(_naturalWeapons.Count > 0)
                {
                    foreach(var weapon in _naturalWeapons)
                    {
                        if (AddItemToEquipment(weapon))
                            break;
                    }
                }
                return true;
            }
            return false;
        }

        private bool FindEmptySlots(Item[] slots, out List<int> emptyAt)
        {
            emptyAt = new List<int>();
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i] == null)
                {
                    emptyAt.Add(i);
                    return true;
                }
            }
            return false;
        }

        private bool FindItemInSlots(Item[] slots, Item item, out int foundAt)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if(slots[i] == item)
                {
                    foundAt = i;
                    return true;
                }
            }
            foundAt = 1000;
            return false;
        }
        #endregion
    }
}
