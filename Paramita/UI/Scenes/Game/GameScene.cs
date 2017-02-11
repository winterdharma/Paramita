using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic;
using Paramita.UI.Input;
using Paramita.UI.Scenes.Game;

namespace Paramita.UI.Scenes
{

    public class GameScene : Scene
    {
        private Dungeon _dungeon;
        private TileMapPanel _tileMapPanel;        
        private static StatusPanel _statusPanel;
        private InventoryPanel _inventoryPanel;

        private Texture2D inventory_background;




        public GameScene(GameController game) : base(game)
        {
        }




        public override void Initialize()
        {
            base.Initialize(); // This calls LoadContent()
            InputResponder.SubscribeToInputEvents();
            _dungeon = new Dungeon();

            _tileMapPanel = new TileMapPanel(_dungeon.GetCurrentLevelTiles(), 
                _dungeon.GetCurrentLevelItems(), _dungeon.GetCurrentLevelActors());
            _statusPanel = new StatusPanel(GameController.ArialBold, 10, new Point(0,720));
            _inventoryPanel = new InventoryPanel(Dungeon.Player, inventory_background, 10);
        }


        protected override void LoadContent()
        {
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Floor, content.Load<Texture2D>("Images\\Tiles\\floor"));
            // Door tiles not yet implemented
            //TileMapPanel.Spritesheets.Add(SpriteType.Tile_Door, content.Load<Texture2D>("Images\\Tiles\\door"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_Wall, content.Load<Texture2D>("Images\\Tiles\\wall"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsUp, content.Load<Texture2D>("Images\\Tiles\\stairs_up"));
            TileMapPanel.Spritesheets.Add(SpriteType.Tile_StairsDown, content.Load<Texture2D>("Images\\Tiles\\stairs_down"));
            TileMapPanel.Spritesheets.Add(SpriteType.Item_Coins, content.Load<Texture2D>("Images\\Items\\coins"));
            TileMapPanel.Spritesheets.Add(SpriteType.Item_Meat, content.Load<Texture2D>("Images\\Items\\meat"));
            TileMapPanel.Spritesheets.Add(SpriteType.Item_Buckler, content.Load<Texture2D>("Images\\Items\\buckler"));
            TileMapPanel.Spritesheets.Add(SpriteType.Item_ShortSword, content.Load<Texture2D>("Images\\Items\\short_sword"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_GiantRat, content.Load<Texture2D>("Images\\SentientBeings\\giant_rat"));
            TileMapPanel.Spritesheets.Add(SpriteType.Actor_Player, content.Load<Texture2D>("Images\\SentientBeings\\human_player"));

            inventory_background = content.Load<Texture2D>("black_background1");
        }


        public override void Update(GameTime gameTime)
        {
            // update the UI panels
            _dungeon.Update();
            _tileMapPanel.Update(gameTime);
            _statusPanel.Update(gameTime);
            _inventoryPanel.Update(gameTime);

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            _tileMapPanel.Draw(gameTime, GameRef.SpriteBatch);
            _statusPanel.Draw(gameTime, GameRef.SpriteBatch);
            _inventoryPanel.Draw(gameTime, GameRef.SpriteBatch);
        }

        public static void PostNewStatus(string message)
        {
            _statusPanel.AddMessage(message);
        }
    }
}
