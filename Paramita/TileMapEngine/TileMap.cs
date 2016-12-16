using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Paramita.TileMapEngine
{
    public class TileMap
    {
        string mapName;
        TileLayer groundLayer;
        TileLayer edgeLayer;
        TileLayer buildingLayer;
        TileLayer decorationLayer;
        Dictionary<string, Point> characters;
        [ContentSerializer]
        int mapWidth;
        [ContentSerializer]
        int mapHeight;
        TileSet tileSet;


        [ContentSerializer]
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        [ContentSerializer]
        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        [ContentSerializer]
        public TileLayer GroundLayer
        {
            get { return groundLayer; }
            set { groundLayer = value; }
        }

        [ContentSerializer]
        public TileLayer EdgeLayer
        {
            get { return edgeLayer; }
            set { edgeLayer = value; }
        }

        [ContentSerializer]
        public TileLayer BuildingLayer
        {
            get { return buildingLayer; }
            set { buildingLayer = value; }
        }

        [ContentSerializer]
        public Dictionary<string, Point> Characters
        {
            get { return characters; }
            private set { characters = value; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * TileEngine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * TileEngine.TileHeight; }
        }



    
        private TileMap()
        {
        }


        private TileMap(TileSet tileSet, string mapName)
        {
            this.characters = new Dictionary<string, Point>();
            this.tileSet = tileSet;
            this.mapName = mapName;
        }


        public TileMap(TileSet tileSet, TileLayer groundLayer, TileLayer edgeLayer, TileLayer buildingLayer, 
            TileLayer decorationLayer, string mapName) : this(tileSet, mapName)
        {
            this.groundLayer = groundLayer;
            this.edgeLayer = edgeLayer;
            this.buildingLayer = buildingLayer;
            this.decorationLayer = decorationLayer;
            mapWidth = groundLayer.Width;
            mapHeight = groundLayer.Height;
        }




        public void SetGroundTile(int x, int y, int index)
        {
            groundLayer.SetTile(x, y, index);
        }


        public int GetGroundTile(int x, int y)
        {
            return groundLayer.GetTile(x, y);
        }


        public void SetEdgeTile(int x, int y, int index)
        {
            edgeLayer.SetTile(x, y, index);
        }


        public int GetEdgeTile(int x, int y)
        {
            return edgeLayer.GetTile(x, y);
        }


        public void SetBuildingTile(int x, int y, int index)
        {
            buildingLayer.SetTile(x, y, index);
        }


        public int GetBuildingTile(int x, int y)
        {
            return buildingLayer.GetTile(x, y);
        }


        public void SetDecorationTile(int x, int y, int index)
        {
            decorationLayer.SetTile(x, y, index);
        }


        public int GetDecorationTile(int x, int y)
        {
            return decorationLayer.GetTile(x, y);
        }


        public void FillEdges()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    edgeLayer.SetTile(x, y, -1);
                }
            }
        }


        public void FillBuilding()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    buildingLayer.SetTile(x, y, -1);
                }
            }
        }


        public void FillDecoration()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    decorationLayer.SetTile(x, y, -1);
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            if (groundLayer != null)
                groundLayer.Update(gameTime);
            if (edgeLayer != null)
                edgeLayer.Update(gameTime);
            if (buildingLayer != null)
                buildingLayer.Update(gameTime);
            if (decorationLayer != null)
                decorationLayer.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            if (groundLayer != null)
                groundLayer.Draw(gameTime, spriteBatch, tileSet, camera);
            if (edgeLayer != null)
                edgeLayer.Draw(gameTime, spriteBatch, tileSet, camera);
            if (buildingLayer != null)
                buildingLayer.Draw(gameTime, spriteBatch, tileSet, camera);
            if (decorationLayer != null)
                decorationLayer.Draw(gameTime, spriteBatch, tileSet, camera);
        }
    }
}
