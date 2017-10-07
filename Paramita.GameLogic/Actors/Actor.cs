using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;

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
        None,
        GiantRat,
        HumanPlayer
    }

    public abstract class Actor : IEquatable<Actor>
    {
        #region Fields
        protected string name;
        #endregion


        #region Events
        public event EventHandler<InventoryChangeEventArgs> OnInventoryChange;
        public event EventHandler<DirectionEventArgs> OnMoveAttempt;
        public event EventHandler<StatusMessageEventArgs> OnStatusMsgSent;
        #endregion


        public Actor(ActorType actorType, List<int> combatData)
        {
            ActorType = actorType;
            Facing = Compass.East;
            Inventory = new Inventory();
            Combatant = new Combatant(combatData, Inventory);
        }


        #region Properties
        protected string Name
        {
            set { name = value; Combatant.Name = value; }
        }
        public ActorType ActorType { get; protected set; }

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
            
            OnMoveAttempt?.Invoke(this, new DirectionEventArgs(direction));

            Tile newTile = CurrentTile;
            if (newTile == currentTile)
                return false;
            else
                return true;
        }
        #endregion


        #region Helper Methods
        protected void SubscribeToEvents()
        {
            Inventory.OnInventoryChange += HandleInventoryChange;
            Combatant.OnAttackResolved += HandleAttackResolution;
            Combatant.OnCombatantKilled += HandleActorDead;
        }

        private void HandleInventoryChange(object sender, InventoryChangeEventArgs eventArgs)
        {
            OnInventoryChange?.Invoke(this, eventArgs);
        }

        private void HandleAttackResolution(object sender, AttackEventArgs eventArgs)
        {
            OnStatusMsgSent?.Invoke(this, new StatusMessageEventArgs(eventArgs.Report));
        }

        private void HandleActorDead(object sender, EventArgs eventArgs)
        {
            IsDead = true;
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