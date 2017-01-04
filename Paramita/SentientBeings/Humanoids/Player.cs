using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;
using System.Collections.Generic;
using Paramita.Items.Valuables;
using Paramita.Items.Consumables;

namespace Paramita.SentientBeings
{
    public class Player : Humanoid, IContainer
    {
        // private fields
        private InputDevices inputDevices;

        // Currently not in use, saved for future
        //private AnimatedSprite sprite;
        //Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

        private int gold;
        private int sustanence;

        // Not currently used, but saved for future time when animations may be needed
        //public AnimatedSprite Sprite
        //{
        //    get { return sprite; }
        //}

        //public Dictionary<AnimationKey, Animation> PlayerAnimations
        //{
        //    get { return animations; }
        //}

        public Item[] Items { get { return items; } }

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



        public Player(GameController game, string name, Texture2D sprites, Rectangle right, Rectangle left) 
            : base(game.GameScene, sprites, right, left)
        {
            inputDevices = game.InputDevices;
            this.name = name;
            Gold = 0;
            sustanence = 240;

            items = new Item[5];

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

            naturalWeapons = new List<Weapon>();
            naturalWeapons.Add(ItemCreator.CreateFist());

            EquipItem(naturalWeapons[0]);

            attacks = new List<Weapon>();
            UpdateAttacks();
        }



        // Will be used when saved games are implemented
        public void SavePlayer() { }




        public void Initialize()
        {
            // Not currently used, but saved for future
            //Animation animation = new Animation(3, 32, 32, 0, 0);
            //animations.Add(AnimationKey.WalkDown, animation);

            //animation = new Animation(3, 32, 32, 0, 32);
            //animations.Add(AnimationKey.WalkLeft, animation);

            //animation = new Animation(3, 32, 32, 0, 64);
            //animations.Add(AnimationKey.WalkRight, animation);

            //animation = new Animation(3, 32, 32, 0, 96);
            //animations.Add(AnimationKey.WalkUp, animation);

            //sprite = new AnimatedSprite(texture, PlayerAnimations);
            //sprite.CurrentAnimation = AnimationKey.WalkDown;
            //sprite.IsAnimating = false;

            //base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);
        }


        // This method polls InputDevices for player input and responds
        private void HandleInput()
        {
            // check for movement input and store the direction returned
            Facing = inputDevices.Moved();

            Tile tile = gameScene.Map.GetTile(CurrentTile.TilePoint + Direction.GetPoint(Facing));

            SentientBeing npc = gameScene.GetNpcOnTile(tile);
            if (npc is SentientBeing)
            {
                Attack(npc);
                gameScene.IsPlayersTurn = false;
            }
            else
            {
                bool moved = MoveTo(Facing);
                if (moved == true)
                {
                    CheckForNewTileEvents();
                    // burn a calorie while walking
                    ExpendSustanence();
                    // toggle the turn flag so Npcs will go next
                    gameScene.IsPlayersTurn = false;
                }
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

                if (items[0] is Coins)
                {
                    gameScene.PostNewStatus("You picked up " + items[0].ToString() + ".");
                    gameScene.PostNewStatus("You now have " + Gold + " gold.");
                }
                else if(items[0] is Meat)
                {
                    gameScene.PostNewStatus("You picked up " + items[0].ToString() + ".");
                    gameScene.PostNewStatus("You now have " + Sustanence + " calories of energy left.");
                }
                else
                    gameScene.PostNewStatus("You picked up a " + items[0].ToString() + ".");
            }
        }



        // Checks the TileType of Player.CurrentTile for any events that when moving to them
        private void CheckForTileTypeEvent(TileType tileType)
        {
            switch(tileType)
            {
                case TileType.StairsUp:
                    if(gameScene.LevelNumber > 1)
                        gameScene.ChangeLevel(-1);
                    break;
                case TileType.StairsDown:
                    gameScene.ChangeLevel(1);
                    break;
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
                gameScene.PostNewStatus(message);
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
                gameScene.PostNewStatus("You dropped a " + itemToDrop.ToString() + ".");
            }
        }



        // Draws the player onto the tilemap
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
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



        // This method removes an item from the player's possessions
        public Item RemoveItem(Item item)
        {
            for(int x = 0; x < items.Length; x++)
            {
                if(items[x] != null && items[x].Equals(item))
                {
                    items[x] = null;
                    return item;
                }
            }

            if(leftHand != null && leftHand.Equals(item))
            {
                leftHand = null;
                return item;
            }
            else if(rightHand != null && rightHand.Equals(item))
            {
                rightHand = naturalWeapons[0];
                return item;
            }
            else if(head != null && head.Equals(item))
            {
                head = null;
                return item;
            }
            else if(body != null && body.Equals(item))
            {
                body = null;
                return item;
            }
            else if(feet != null && feet.Equals(item))
            {
                feet = null;
                return item;
            }

            return null;
        }


        // This method accepts an item and adds it to the player's possessions
        public bool AddItem(Item item)
        {
            if(item is Coins)
            {
                var coins = item as Coins;
                Gold = Gold + coins.Number;
                return true;
            }

            bool equiped = EquipItem(item);

            if(equiped == false)
            {
                for (int x = 0; x < items.Length; x++)
                {
                    if (items[x] == null)
                    {
                        items[x] = item;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }



        // This method returns an array of the items in the player's inventory
        public Item[] InspectItems()
        {
            return items;
        }



        // Provides a string equivalent of the player
        public override string ToString()
        {
            return name;
        }
    }
}
