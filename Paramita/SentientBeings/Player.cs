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

        public Point CurrentTile {
            get
            {
                return new Point(
                    (int)Position.X / gameRef.GameScene.TileSet.TileSize,
                    (int)Position.Y / gameRef.GameScene.TileSet.TileSize);
            }
        }

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

            // declare the new position (in pixels), defaulted to zero (means no change)
            Vector2 newPosition = Vector2.Zero;
            // find the coordinates of the tile the player is trying to move to
            Point newTile = CurrentTile + Direction.GetPoint(Facing);
            // try to move there - if successful, the @newPosition has the sprite's new pixel coords
            newPosition = TryToMoveTo(newTile);

            // if there was a successful move, change the sprite's Position
            if (newPosition != Vector2.Zero)
            {
                Sprite.Position = newPosition;
                if(gameRef.GameScene.Map.GetTile(CurrentTile.X, CurrentTile.Y).InspectItems().Length > 0)
                {
                    TryToPickUpItem();
                }
            }


            Sprite.IsAnimating = false;  // turn off animations for now
            
            Sprite.LockToMap(new Point(gameRef.GameScene.Map.WidthInPixels, 
                    gameRef.GameScene.Map.HeightInPixels));  // change the camera position
        }

        private Vector2 TryToMoveTo(Point destTile)
        {
            if(gameRef.GameScene.Map.GetTile(destTile.X,destTile.Y).IsWalkable == true)
            {
                return new Vector2(
                    destTile.X * gameRef.GameScene.TileSet.TileSize,
                    destTile.Y * gameRef.GameScene.TileSet.TileSize);
            }

            return Vector2.Zero;
        }

        private void TryToPickUpItem()
        {
            Tile tile = gameRef.GameScene.Map.GetTile(CurrentTile.X, CurrentTile.Y);
            Item[] items = tile.InspectItems();
            if(AddItem(items[0]))
            {
                tile.RemoveItem(items[0]);
            }
        }

        private void DropItem()
        {
            Tile tile = gameRef.GameScene.Map.GetTile(CurrentTile);
            Item item = RemoveItem(inventory[0]);
            if (item != null)
            {
                Point dropPoint = CurrentTile + Direction.GetPoint(GetDirectionFacing(Sprite.CurrentAnimation));
                gameRef.GameScene.Map.GetTile(dropPoint).AddItem(item);
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
