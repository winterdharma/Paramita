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


    public class Player : Humanoid, IContainer
    {
        private int gold;
        private int sustanence;

        public Item[] UnequipedItems { get { return unequipedItems; } }
        public Item[] EquipedItems { get { return equipedItems; } }

        public int Gold
        {
            get { return gold; }
            private set { gold = value; }
        }

        public int Sustanence
        {
            get { return sustanence; }
            private set { sustanence = value; }
        }

        public event EventHandler<LevelChangeEventArgs> OnLevelChange;
        public event EventHandler<InventoryChangeEventArgs> OnInventoryChange;

        public Player(string name) : base(BeingType.HumanPlayer)
        {
            this.name = name;

            InitializeAttributes();
            InitializeItemLists();
        }


        protected override void InitializeAttributes()
        {
            Gold = 0;
            sustanence = 240;
            hitPoints = 10;
            protection = 0;
            magicResistance = 10;
            strength = 10;
            attackSkill = 10;
            defenseSkill = 10;
            precision = 10;
            morale = 10;
            encumbrance = 0;
            fatigue = 0;
            size = 2;
        }



        protected override void InitializeItemLists()
        {
            base.InitializeItemLists();

            naturalWeapons = new List<Weapon>();
            naturalWeapons.Add(ItemCreator.CreateFist());

            TryToEquipItem(naturalWeapons[0]);

            attacks = new List<Weapon>();
            UpdateAttacks();

            shields = new List<Shield>();
        }



        // Will be used when saved games are implemented
        public void SavePlayer() { }


        public Tuple<Dictionary<string, ItemType>, int> GetInventory()
        {
            var playerInventory = new Dictionary<string, ItemType>();

            playerInventory["left_hand"] = LeftHandItem != null ? LeftHandItem.ItemType : ItemType.None;
            playerInventory["right_hand"] = RightHandItem != null ? RightHandItem.ItemType : ItemType.None;
            playerInventory["head"] = HeadItem != null ? HeadItem.ItemType : ItemType.None;
            playerInventory["body"] = BodyItem != null ? BodyItem.ItemType : ItemType.None;
            playerInventory["feet"] = FeetItem != null ? FeetItem.ItemType : ItemType.None;

            Item[] playerOtherItems = UnequipedItems;
            string label; int num = 0;
            foreach (var item in playerOtherItems)
            {
                num++;
                label = "other" + num;
                playerInventory[label] = item != null ? item.ItemType : ItemType.None;
            }

            return new Tuple<Dictionary<string, ItemType>, int>(playerInventory, Gold);
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



        // checks to see if there is an item to pick up and does so if one is there.
        // used when player moves to a new tile.
        private void TryToPickUpItem()
        {
            Item[] items = CurrentTile.InspectItems();

            if (AddItem(items[0]) == true)
            {
                CurrentTile.RemoveItem(items[0]);
                OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(Dungeon.GetPlayerInventory() ));

                if (items[0] is Coins)
                {
                    //GameScene.PostNewStatus("You picked up " + items[0].ToString() + ".");
                    //GameScene.PostNewStatus("You now have " + Gold + " gold.");
                }
                else { }
                    //GameScene.PostNewStatus("You picked up a " + items[0].ToString() + ".");
            }
        }



        // Checks the TileType of Player.CurrentTile for any events that when moving to them
        private void CheckForTileTypeEvent(TileType tileType)
        {
            int change = 0;
            switch(tileType)
            {
                case TileType.StairsUp:
                    if (Dungeon.CurrentLevelNumber > 1)
                        change = -1;
                    break;
                case TileType.StairsDown:
                    change = 1;
                    break;
            }
            if(change != 0)
            {
                OnLevelChange?.Invoke(this, new LevelChangeEventArgs(change));
            }
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
            else if(Sustanence == 120)
            {
                message = "You are getting hungry (120).";
            }
            
            if(message != "")
            {
                //GameScene.PostNewStatus(message);
            }
        }



        // Drops item onto the tile the player is located in.
        // The item to drop is indicated by @itemToDrop, which should be a valid
        // index value for the player's inventory list
        public void DropItem(Item itemToDrop)
        {
            if (itemToDrop != null && !(itemToDrop is NaturalWeapon))
            {
                RemoveItem(itemToDrop);
                OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(Dungeon.GetPlayerInventory()));
                CurrentTile.AddItem(itemToDrop);
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
                    if (itemType.Equals(UnequipedItems[0].ItemType))
                        item = UnequipedItems[0];
                    break;
                case "other2":
                    if (itemType.Equals(UnequipedItems[1].ItemType))
                        item = UnequipedItems[1];
                    break;
                case "other3":
                    if (itemType.Equals(UnequipedItems[2].ItemType))
                        item = UnequipedItems[2];
                    break;
                case "other4":
                    if (itemType.Equals(UnequipedItems[3].ItemType))
                        item = UnequipedItems[3];
                    break;
                case "other5":
                    if (itemType.Equals(UnequipedItems[4].ItemType))
                        item = UnequipedItems[4];
                    break;
            }

            if (item != null)
            {
                DropItem(item);
            }
            else
                throw new Exception("Player.DropItem(string,string) found no item to drop.");
        }

        public override bool AddItem(Item item)
        {
            if (AddCoins(item))
                return true;

            return base.AddItem(item);
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

        public override string GetDescription()
        {
            throw new NotImplementedException("Player.GetDescription() is not implemented yet.");
        }
    }
}
