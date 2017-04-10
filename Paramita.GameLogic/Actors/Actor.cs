using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;
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
     * The Inventory and Combatant classes contain Properties and methods related
     * to possessing items and combat with other actors.
     */

    public enum ActorType
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
        #endregion


        #region Events
        public event EventHandler<InventoryChangeEventArgs> OnInventoryChange;
        public event EventHandler<MoveEventArgs> OnMoveAttempt;
        public event EventHandler<StatusMessageEventArgs> OnStatusMsgSent;
        public event EventHandler<MoveEventArgs> OnActorDeath;
        #endregion


        public Actor(ActorType beingType, List<int> combatData)
        {
            BeingType = beingType;
            Facing = Compass.East;
            Inventory = new Inventory();
            Combatant = new Combatant(combatData);
        }


        #region Properties
        protected string Name
        {
            set { name = value; Combatant.Name = value; }
        }
        public ActorType BeingType { get; protected set; }

        public Tile CurrentTile { get; set; }

        public Compass Facing { get; private set; }

        public Inventory Inventory { get; protected set; }

        public Combatant Combatant { get; protected set; }

        public int TimesAttacked
        {
            get { return Combatant.TimesAttacked; }
            set { Combatant.TimesAttacked = value; }
        }

        public bool IsDead { get; set; }
        #endregion


        #region Abstract Methods
        // helper methods for child class constructors
        protected abstract void InitializeInventory();
        // This method is the verbose report on a sentient being
        public abstract string GetDescription();
        #endregion


        #region Combat Related API
        public void Attack(Combatant defender)
        {
            Combatant.Attack(defender);
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
            if(Combatant.Fatigue > 0)
                Combatant.Fatigue--;
        }

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
            int total = Combatant.Encumbrance;

            foreach(var item in Inventory.Equipment)
            {
                total += GetItemEncumbrance(item);
            }

            return total;
        }
        #endregion


        #region Helper Methods
        protected void SubscribeToEvents()
        {
            Inventory.OnInventoryChange += HandleInventoryChange;
            Inventory.OnWeaponsChange += HandleWeaponsChange;
            Combatant.OnAttackResolved += HandleAttackResolution;
            Combatant.OnCombatantKilled += HandleActorDead;
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChange?.Invoke(this, eventArgs);
            Combatant.Shields = Inventory.Shields;
            Combatant.TotalEncumbrance = GetTotalEncumbrance();
        }

        private void HandleWeaponsChange(object sender, EventArgs e)
        {
            Combatant.UpdateAttacks(Inventory.Weapons);
        }

        private void HandleAttackResolution(object sender, AttackEventArgs eventArgs)
        {
            OnStatusMsgSent(this, new StatusMessageEventArgs(eventArgs.Report));
        }

        private void HandleActorDead(object sender, EventArgs eventArgs)
        {
            IsDead = true;
            OnActorDeath?.Invoke(this, new MoveEventArgs(Compass.None, CurrentTile.TilePoint, Point.Zero));
        }

        private int GetItemEncumbrance(Item item)
        {
            int encumbrance = 0;
            if (item is Armor)
            {
                encumbrance = (item as Armor).Encumbrance;
            }
            else if (item is Shield)
            {
                encumbrance = (item as Shield).Encumbrance;
            }
            else if (item is Weapon)
            {
                encumbrance = (item as Weapon).Encumbrance;
            }
            return encumbrance;
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
