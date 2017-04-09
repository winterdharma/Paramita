using Paramita.GameLogic.Items;
using System.Collections.Generic;
using Paramita.GameLogic.Items.Valuables;
using System;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic.Actors
{

    public class LevelChangeEventArgs : EventArgs
    {
        public int LevelChange { get; }

        public LevelChangeEventArgs(int levelChange)
        {
            LevelChange = levelChange;
        }
    }


    public class Player : Humanoid, IContainer, IPlayer
    {
        #region Fields
        private int _gold;
        private int _sustanence;
        #endregion


        #region Properties
        public int Gold
        {
            get { return _gold; }
            private set { _gold = value; }
        }

        public int Sustanence
        {
            get { return _sustanence; }
            private set { _sustanence = value; }
        }
        #endregion


        public event EventHandler<LevelChangeEventArgs> OnLevelChange;

        public Player(string name) 
            : base(BeingType.HumanPlayer, new List<int>() { 10, 0, 10, 10, 10, 10, 10, 10, 0, 2 })
        {
            SubscribeToEvents();
            Name = name;
            Gold = 0;
            _sustanence = 240; 
            InitializeItemLists();
        }

        #region Protected Methods
        private void InitializeItemLists()
        {
            Inventory.NaturalWeapons.Add(ItemCreator.CreateFist());
            Inventory.NaturalWeapons.Add(ItemCreator.CreateFist());
            Inventory.AddToWeapons(Inventory.NaturalWeapons);
        }
        #endregion


        // Will be used when saved games are implemented
        public void SavePlayer() { }


        public void GetInventory()
        {
            Inventory.RaiseChangeEvent();
        }

        public override void Update()
        {
            base.Update();
        }

        // This method handles player input actions (currently only movement)
        public void HandleInput(Compass direction)
        {
            if (MoveTo(direction))
            {
                CheckForNewTileEvents();
                ExpendSustanence();
            }
        }

        public void DropItem(Item itemToDrop)
        {
            if (!(itemToDrop is NaturalWeapon))
            {
                Discard(itemToDrop);
                //GameScene.PostNewStatus("You dropped a " + itemToDrop.ToString() + ".");
            }
        }

        // This method is called by Dungeon when a UI event calls for the player to drop an item
        public void DropItem(string slot, ItemType itemType)
        {
            Item item = null;
            switch (slot)
            {
                case "left_hand":
                    if (itemType.Equals(LeftHandItem.ItemType))
                        item = LeftHandItem;
                    break;
                case "right_hand":
                    if (itemType.Equals(RightHandItem.ItemType))
                        item = RightHandItem;
                    break;
                case "head":
                    if (itemType.Equals(HeadItem.ItemType))
                        item = HeadItem;
                    break;
                case "body":
                    if (itemType.Equals(BodyItem.ItemType))
                        item = BodyItem;
                    break;
                case "feet":
                    if (itemType.Equals(FeetItem.ItemType))
                        item = FeetItem;
                    break;
                case "other1":
                    if (itemType.Equals(Inventory.Storage[0].ItemType))
                        item = Inventory.Storage[0];
                    break;
                case "other2":
                    if (itemType.Equals(Inventory.Storage[1].ItemType))
                        item = Inventory.Storage[1];
                    break;
                case "other3":
                    if (itemType.Equals(Inventory.Storage[2].ItemType))
                        item = Inventory.Storage[2];
                    break;
                case "other4":
                    if (itemType.Equals(Inventory.Storage[3].ItemType))
                        item = Inventory.Storage[3];
                    break;
                case "other5":
                    if (itemType.Equals(Inventory.Storage[4].ItemType))
                        item = Inventory.Storage[4];
                    break;
            }

            if (item != null)
            {
                DropItem(item);
            }
            else
                throw new Exception("Found no matching item to drop.");
        }

        public override bool AddItem(Item item)
        {
            if (AddCoins(item))
                return true;

            return Acquire(item);
        }

        public override string GetDescription()
        {
            throw new NotImplementedException("Player.GetDescription() is not implemented yet.");
        }

        #region Helper Methods
        /*  
         *  This method is called immediately when CurrentTile is changed in Update() to check for
         *  any events that are triggered by the new tile, such as picking up items, changing levels, etc.
         */
        private void CheckForNewTileEvents()
        {
            // check for items in the new tile and pick the first item present
            if (CurrentTile.InspectItems().Length > 0)
                TryToPickUpItem();
            // check for events based on moving to TileTypes like StairsUp & StairsDown
            CheckForTileTypeEvent(CurrentTile.TileType);
        }

        // Checks the TileType of Player.CurrentTile for any events that when moving to them
        private void CheckForTileTypeEvent(TileType tileType)
        {
            int change = 0;
            switch (tileType)
            {
                case TileType.StairsUp:
                    if (Dungeon.CurrentLevelNumber > 1)
                        change = -1;
                    break;
                case TileType.StairsDown:
                    change = 1;
                    break;
            }
            if (change != 0)
            {
                OnLevelChange?.Invoke(this, new LevelChangeEventArgs(change));
            }
        }

        // checks to see if there is an item to pick up and does so if one is there.
        // used when player moves to a new tile.
        private void TryToPickUpItem()
        {
            Item[] items = CurrentTile.InspectItems();

            if (AddItem(items[0]) == true)
            {
                CurrentTile.RemoveItem(items[0]);

                if (items[0] is Coins)
                {
                    //GameScene.PostNewStatus("You picked up " + items[0].ToString() + ".");
                    //GameScene.PostNewStatus("You now have " + Gold + " gold.");
                }
                else { }
                //GameScene.PostNewStatus("You picked up a " + items[0].ToString() + ".");
            }
        }

        private bool AddCoins(Item item)
        {
            if (item is Coins)
            {
                var coins = item as Coins;
                Gold = Gold + coins.Number;
                return true;
            }
            return false;
        }

        // decrement's the player's sustanence value and writes messages to
        // GameScene.StatusMessages
        private void ExpendSustanence()
        {
            Sustanence = Sustanence - 1;
            string message = "";

            if (Sustanence == 0)
            {
                message = "You died of starvation (0).";
            }
            else if (Sustanence == 30)
            {
                message = "You are starving and need to find food soon (30).";
            }
            else if (Sustanence == 120)
            {
                message = "You are getting hungry (120).";
            }

            if (message != "")
            {
                //GameScene.PostNewStatus(message);
            }
        }
        #endregion
    }
}
