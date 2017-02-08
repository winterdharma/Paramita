using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using System.Collections.Generic;
using Paramita.Items.Valuables;
using System;
using Paramita.Levels;
using Paramita.UI.Input;
using Paramita.Mechanics;

namespace Paramita.SentientBeings
{
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



        public Player(Level level, string name, Texture2D sprites) 
            : base(level, sprites)
        {
            this.name = name;

            InputDevices.OnLeftKeyWasPressed += HandleMoveLeftInput;
            InputDevices.OnRightKeyWasPressed += HandleMoveRightInput;
            InputDevices.OnUpKeyWasPressed += HandleMoveUpInput;
            InputDevices.OnDownKeyWasPressed += HandleMoveDownInput;

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


        public override void Update(GameTime gameTime)
        {
            //HandleInput();
            base.Update(gameTime);
        }


        // This method polls InputDevices for player input and responds
        private void HandleInput(Compass direction)
        {
            Tile tile = level.TileMap.GetTile(CurrentTile.TilePoint + Direction.GetPoint(direction));

            SentientBeing npc = LevelManager.CurrentLevel.GetNpcOnTile(tile);
            if (npc is SentientBeing)
            {
                Attack(npc);
                LevelManager.CurrentLevel.IsPlayersTurn = false;
            }
            else
            {
                bool moved = MoveTo(direction);
                if (moved)
                {
                    CheckForNewTileEvents();
                    // burn a calorie while walking
                    ExpendSustanence();
                    // toggle the turn flag so Npcs will go next
                    LevelManager.CurrentLevel.IsPlayersTurn = false;
                }
            }
        }

        private void HandleMoveLeftInput(object sender, EventArgs e)
        {
            Compass direction = Compass.West;
            HandleInput(direction);
        }

        private void HandleMoveRightInput(object sender, EventArgs e)
        {
            Compass direction = Compass.East;
            HandleInput(direction);
        }

        private void HandleMoveUpInput(object sender, EventArgs e)
        {
            Compass direction = Compass.North;
            HandleInput(direction);
        }

        private void HandleMoveDownInput(object sender, EventArgs e)
        {
            Compass direction = Compass.South;
            HandleInput(direction);
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

                if (items[0] is Coins)
                {
                    GameScene.PostNewStatus("You picked up " + items[0].ToString() + ".");
                    GameScene.PostNewStatus("You now have " + Gold + " gold.");
                }
                else
                    GameScene.PostNewStatus("You picked up a " + items[0].ToString() + ".");
            }
        }



        // Checks the TileType of Player.CurrentTile for any events that when moving to them
        private void CheckForTileTypeEvent(TileType tileType)
        {
            switch(tileType)
            {
                case TileType.StairsUp:
                    if(LevelManager.LevelNumber > 1)
                        ChangeLevel(-1);
                    break;
                case TileType.StairsDown:
                    ChangeLevel(1);
                    break;
            }
        }


        private void ChangeLevel(int levelChange)
        {
            LevelManager.ChangeLevel(levelChange, this);
            level = LevelManager.CurrentLevel;

            if (levelChange < 0)
                CurrentTile = level.GetStairsDownTile();
            else if (levelChange > 0)
                CurrentTile = level.GetStairsUpTile();

            GameScene.PostNewStatus("You are now on level " + LevelManager.LevelNumber + ".");
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
                GameScene.PostNewStatus(message);
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
                CurrentTile.AddItem(itemToDrop);
                GameScene.PostNewStatus("You dropped a " + itemToDrop.ToString() + ".");
            }
        }

        public override bool AddItem(Item item)
        {
            if (AddCoins(item) == true)
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
            throw new NotImplementedException();
        }
    }
}
