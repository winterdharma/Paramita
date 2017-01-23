using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Levels;
using Paramita.Scenes.Game;
using Paramita.SentientBeings;
using Paramita.UI;

namespace Paramita.Scenes
{

    public class GameScene : Scene
    {
        private Player player;        
        private static StatusMessages statuses;
        private Inventory inventoryPanel;

        private Texture2D inventory_background;

        public TileMap Map { get { return LevelManager.CurrentLevel.TileMap; } }
        public Level CurrentLevel
        {
            get { return LevelManager.CurrentLevel; }
        }




        public GameScene(GameController game) : base(game)
        {
        }




        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()
            
            LevelFactory.TileSet = new TileSet("tileset1", LevelFactory.Tilesheet, 8, 8, 32);

            SetUpNewGame();

            statuses = new StatusMessages(GameController.ArialBold, 10, new Point(0,720));
            inventoryPanel = new Inventory(player, inventory_background, 10);
        }


        protected override void LoadContent()
        {
            LevelFactory.Tilesheet = content.Load<Texture2D>("tileset1");

            ItemCreator.Spritesheets.Add(ItemType.Coins, content.Load<Texture2D>("Images\\Items\\coins"));
            ItemCreator.Spritesheets.Add(ItemType.Meat, content.Load<Texture2D>("Images\\Items\\meat"));
            ItemCreator.Spritesheets.Add(ItemType.Shield, content.Load<Texture2D>("Images\\Items\\buckler"));
            ItemCreator.Spritesheets.Add(ItemType.ShortSword, content.Load<Texture2D>("Images\\Items\\short_sword"));

            SentientBeingCreator.Spritesheets.Add(BeingType.GiantRat, content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            SentientBeingCreator.Spritesheets.Add(BeingType.HumanPlayer, content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

            inventory_background = content.Load<Texture2D>("black_background1");
        }


        public override void Update(GameTime gameTime)
        {
            // update the UI panels
            statuses.Update(gameTime);
            inventoryPanel.Update(gameTime);
            CurrentLevel.Update(gameTime);

            //move the camera to center on the player
            Camera.LockToSprite(Map, player.Sprite.Position, GameController.ScreenRectangle);

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            CurrentLevel.Draw(gameTime, GameRef.SpriteBatch);
            

            statuses.Draw(gameTime, GameRef.SpriteBatch);
            inventoryPanel.Draw(gameTime, GameRef.SpriteBatch);
        }


        public void SetUpNewGame()
        {
            int levelNumber = LevelManager.LevelNumber =  1;
            var firstLevel = LevelManager.Create(levelNumber);
            player = SentientBeingCreator.CreateHumanPlayer(firstLevel);
            firstLevel.Player = player;
            player.CurrentTile = firstLevel.GetStairsUpTile();
            LevelManager.CurrentLevel = firstLevel;
        }


        public void LoadSavedGame()
        {
            // not yet implemented
        }


        public static void PostNewStatus(string message)
        {
            statuses.AddMessage(message);
        }
    }
}
