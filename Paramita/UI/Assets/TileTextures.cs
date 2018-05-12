using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Assets;
using Paramita.GameLogic.Levels;

namespace Paramita.UI.Assets
{
    public class TileTextures
    {
        public static void Add(TileType type, Texture2D texture)
        {
            Textures.Textures2D[(int)type] = texture;
        }

        public static Texture2D Get(TileType type)
        {
            return Textures.Textures2D[(int)type];
        }
    }
}
