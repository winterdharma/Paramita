using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Mechanics;
using Paramita.Scenes;

namespace Paramita.SentientBeings
{
    public class Player : SentientBeing, IContainer
    {
        private GameController gameRef;
        private string name;
        private bool gender;
        private AnimatedSprite sprite;
        private Texture2D texture;
        private Item[] inventory;

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public Tile CurrentTile { get; set; }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }




        private Player(GameController game) : base(game) { }

        public Player(GameController game, string name, bool gender, Texture2D texture) : base(game)
        {
            gameRef = game;
            this.name = name;
            this.gender = gender;
            this.texture = texture;
            sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);
            sprite.CurrentAnimation = AnimationKey.WalkDown;
            inventory = new Item[10];
            sprite.IsAnimating = false;
        }




        public void SavePlayer() { }

        public static Player Load(GameController game)
        {
            Player player = new Player(game);
            return player;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            Position = gameRef.GameScene.Map.GetTilePosition(CurrentTile);
            Sprite.Update(gameTime);
            base.Update(gameTime);
        }


        private void HandleInput()
        {
            // check if item was dropped
            if(gameRef.InputDevices.DroppedItem())
                DropItem();

            // get the direction the player moved in (if any)
            Facing = gameRef.InputDevices.MovedTo(); // Facing is in SentientBeing
            
            // set the animation according to the new facing
            if(Facing == Compass.North)
                Sprite.CurrentAnimation = AnimationKey.WalkUp;
            else if(Facing == Compass.South)
                Sprite.CurrentAnimation = AnimationKey.WalkDown;
            else if(Facing == Compass.Northeast || Facing == Compass.East || Facing == Compass.Southeast)
                Sprite.CurrentAnimation = AnimationKey.WalkRight;
            else if(Facing == Compass.Northwest || Facing == Compass.West || Facing == Compass.Southwest)
                Sprite.CurrentAnimation = AnimationKey.WalkLeft;

            // find the coordinates of the tile the player is trying to move to
            Point newTile = CurrentTile.TilePoint + Direction.GetPoint(Facing);
            // try to move there - if successful, update the CurrentTile and check for items picked up
            if(newTile != CurrentTile.TilePoint && TryToMoveTo(newTile))
            {
                CurrentTile = gameRef.GameScene.Map.GetTile(newTile);
                if(gameRef.GameScene.Map.GetTile(CurrentTile.TilePoint).InspectItems().Length > 0)
                    TryToPickUpItem();
                if (CheckForStairsUp() == false)
                    CheckForStairsDown();
            }

            Sprite.LockToMap(new Point(gameRef.GameScene.Map.WidthInPixels, 
                    gameRef.GameScene.Map.HeightInPixels));  // change the camera position
        }

        private bool TryToMoveTo(Point destTile)
        {
            if(gameRef.GameScene.Map.GetTile(destTile).IsWalkable == true)
            {
                return true;
            }
            return false;
        }

        private void TryToPickUpItem()
        {
            Tile tile = gameRef.GameScene.Map.GetTile(CurrentTile.TilePoint);
            Item[] items = tile.InspectItems();
            if(AddItem(items[0]))
            {
                tile.RemoveItem(items[0]);
            }
        }


        private bool CheckForStairsUp()
        {
            if(CurrentTile.TileType == TileType.StairsUp && gameRef.GameScene.LevelNumber > 0)
            {
                gameRef.GameScene.GoUpOneLevel();
                return true;
            }
            return false;
        }

        private void CheckForStairsDown()
        {
            if(CurrentTile.TileType == TileType.StairsDown)
                gameRef.GameScene.GoDownOneLevel();
        }

        private void DropItem()
        {
            Item item = RemoveItem(inventory[0]);
            if (item != null)
            {
                CurrentTile.AddItem(item);
            }
        }

        private Compass GetDirectionFacing(AnimationKey currentAnimation)
        {
            switch(currentAnimation)
            {
                case AnimationKey.WalkUp:
                    return Compass.North;
                case AnimationKey.WalkDown:
                    return Compass.South;
                case AnimationKey.WalkLeft:
                    return Compass.West;
                case AnimationKey.WalkRight:
                    return Compass.East;
            }
            return Compass.None;
        }

        public override void Draw( GameTime gameTime )
        {
            base.Draw(gameTime);
        }



        /*
         * IContainer interface methods
         */

        public Item RemoveItem(Item item)
        {
            for(int x = 0; x < inventory.Length; x++)
            {
                if(inventory[x].Equals(item))
                {
                    inventory[x] = null;
                    return item;
                }
            }
            return null;
        }

        public bool AddItem(Item item)
        {
            for(int x = 0; x < inventory.Length; x++)
            {
                if(inventory[x] == null)
                {
                    inventory[x] = item;
                    return true;
                }
            }
            return false;
        }

        public Item[] InspectItems()
        {
            return inventory;
        }
    }
}
