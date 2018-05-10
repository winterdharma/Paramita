using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Base;
using MonoGameUI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Scenes.Game
{
    public class PMTileMapPanel : TileMap
    {
        public PMTileMapPanel(Scene parent, int draworder, Texture2D tilesheet, Point mapsize, 
            Rectangle view) : base(parent, draworder, tilesheet, mapsize, view)
        {

        }
    }
}
