using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.TileMapEngine
{
    public class TileSet
    {
        public int TilesWide = 8;
        public int TilesHigh = 8;
        public int TileWidth = 64;
        public int TileHeight = 64;

        Texture2D image;
        string imageName;
        Rectangle[] sourceRectangles;



        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return image; } set { image = value; }
        }

        [ContentSerializer]
        public string TextureName
        {
            get { return imageName; } set { imageName = value; }
        }

        [ContentSerializerIgnore]
        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }



        public TileSet()
        {
            sourceRectangles = new Rectangle[TilesWide * TilesHigh];
            int tile = 0;
            for(int x = 0; x < TilesWide; x++)
            {
                sourceRectangles[tile] = 
                    new Rectangle(x * TilesWide, y * TilesHigh, TileWidth, TileHeight);
                tile++;
            }
        }

        public TileSet(int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];
            int tile = 0;
            for (int x = 0; x < TilesWide; x++)
            {
                sourceRectangles[tile] =
                    new Rectangle(x * TilesWide, y * TilesHigh, TileWidth, TileHeight);
                tile++;
            }
        }
    }
}
