using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;
using MonoGameUI.Components;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Scenes.Game
{
    public class PMTileMapPanel : MonoGameUI.Components.TileMap
    {
        #region Fields
        private TileType[,] _tileLayer;
        private ItemType[,] _itemLayer;
        private Tuple<ActorType, Compass, bool>[,] _actorLayer;
        #endregion

        public PMTileMapPanel(Scene parent, int draworder, Point mapsize, Rectangle view) 
            : base(parent, draworder, mapsize, view)
        {
            _viewport = Rectangle;
        }

        #region Initialization
        protected override void Initialize()
        {
            InitializeLevelMap();
            base.Initialize();
        }

        private void InitializeLevelMap()
        {
            var gameScene = (GameScene)Parent;
            var mapLayers = gameScene.Dungeon.GetCurrentLevelLayers();
            InitializeLevelMap(mapLayers);
        }

        private void InitializeLevelMap(
            Tuple<TileType[,], ItemType[,], Tuple<ActorType, Compass, bool>[,]> mapLayers)
        {
            _tileLayer = mapLayers.Item1;
            _itemLayer = mapLayers.Item2;
            _actorLayer = mapLayers.Item3;
        }

        protected override Rectangle UpdatePanelRectangle()
        {
            return _parentRectangle;
        }
        
        protected override Dictionary<string, Element> InitializeElements()
        {
            int layerElements = _tileLayer.GetLength(0) * _tileLayer.GetLength(1);
            var tileLayers = new int[3, layerElements];

            
            for (int x = 0; x < _tileLayer.GetLength(0); x++)
            {
                for(int y = 0; y < _tileLayer.GetLength(1); y++)
                {
                    int index = (y * _tileLayer.GetLength(0)) + x;
                    tileLayers[0, index] = (int)_tileLayer[x, y];
                }
            }

            for (int x = 0; x < _itemLayer.GetLength(0); x++)
            {
                for (int y = 0; y < _itemLayer.GetLength(1); y++)
                {
                    int index = (y * _itemLayer.GetLength(0)) + x;
                    tileLayers[1, index] = (int)_itemLayer[x, y];
                }
            }

            for (int x = 0; x < _actorLayer.GetLength(0); x++)
            {
                for (int y = 0; y < _actorLayer.GetLength(1); y++)
                {
                    int index = (y * _actorLayer.GetLength(0)) + x;
                    tileLayers[2, index] = (int)_actorLayer[x, y].Item1;
                }
            }

            return LoadLayers(tileLayers);
        }
        #endregion
    }
}
