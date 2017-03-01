using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Items;
using System.Collections.Generic;

namespace Paramita.UI.Scenes.Game
{
    public static class ItemTextures
    {
        private static Dictionary<ItemType, Texture2D> _itemTextureMap
            = new Dictionary<ItemType, Texture2D>();

        public static Dictionary<ItemType, Texture2D> ItemTextureMap
        {
            get { return _itemTextureMap; }
            set { _itemTextureMap = value; }
        }
    }
}
