using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


// The TileSet class provides access to the 2D tile images found in a tilesheet.
// The class represents the tiles of a single tilesheet. Multiple TileSets can be
// instantiated to access multiple sheets of tile images.
namespace Paramita.Scenes
{
    public class TileSet
    {
        public int TilesWide;
        public int TilesHigh;
        public int TileSize;
        public int TileHeight;

        Texture2D tilesheet;
        string tilesheetName;
        Rectangle[,] tilesheetRects;



        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return tilesheet; } set { tilesheet = value; }
        }

        [ContentSerializer]
        public string TextureName
        {
            get { return tilesheetName; } set { tilesheetName = value; }
        }

        [ContentSerializerIgnore]
        public Rectangle[,] TilesheetRects
        {
            get { return (Rectangle[,])tilesheetRects.Clone(); }
        }


        /*               CONSTRUCTORS               */

        // @tilesheet = a Texture2D of the tilesheet image file
        // @filename = a label indicating which tilesheet this TileSet represents
        // @tilesWide, @tilesHigh = the number of columns and rows of tiles on the tilesheet
        // @tileSize = size of tiles in pixels (assumes square tiles)
        public TileSet(string filename, Texture2D tilesheet, int tilesWide, int tilesHigh, int tileSize)
        {
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileSize = tileSize;
            TextureName = filename;
            Texture = tilesheet;

            tilesheetRects = new Rectangle[TilesWide,TilesHigh];
            
            for (int x = 0; x < TilesWide; x++)
            {
                for(int y =0; y < TilesHigh; y++)
                {
                    tilesheetRects[x,y] =
                    new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                }
                
            }
        }



        public Rectangle GetRectForTileType(TileType type)
        {
            switch (type)
            {
                case TileType.Door :
                    return tilesheetRects[0, 0];
                case TileType.Floor :
                    return tilesheetRects[0, 5];
                case TileType.Wall :
                    return tilesheetRects[1, 5];
                case TileType.StairsUp:
                    return tilesheetRects[2, 5];
                case TileType.StairsDown:
                    return tilesheetRects[3, 5];
            }

            return tilesheetRects[7, 7];
        }
    }
}
