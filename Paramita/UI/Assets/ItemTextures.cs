using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Assets;
using Paramita.GameLogic.Items;
using System.Collections.Generic;

namespace Paramita.UI.Assets
{
    public class ItemTextures
    {
        public static void Add(ItemType type, Texture2D texture)
        {
            Textures.Textures2D[(int)type] = texture;
        }

        public static Texture2D Get(ItemType type)
        {
            return Textures.Textures2D[(int)type];
        }
    }
}
