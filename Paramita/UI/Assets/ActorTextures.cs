using Microsoft.Xna.Framework.Graphics;
using MonoGameUI.Assets;
using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.UI.Assets
{
    public class ActorTextures : Textures
    {
        public static void Add(ActorType type, Texture2D texture)
        {
            Textures2D[(int)type] = texture;
        }

        public static Texture2D Get(ActorType type)
        {
            return Textures2D[(int)type];
        }
    }
}
