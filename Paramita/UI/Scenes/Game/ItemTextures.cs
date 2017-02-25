using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Paramita.UI.Scenes.Game
{
    public static class ItemTextures
    {
        private static Dictionary<SpriteType, Texture2D> _itemTextureMap
            = new Dictionary<SpriteType, Texture2D>();

        public static Dictionary<SpriteType, Texture2D> ItemTextureMap
        {
            get { return _itemTextureMap; }
            set { _itemTextureMap = value; }
        }
    }
}
