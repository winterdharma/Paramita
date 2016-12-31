using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Scenes;

namespace Paramita.SentientBeings
{
    public class Animal : SentientBeing
    {
        public enum AnimalBodyParts
        {
            Head,
            FrontLeftLeg,
            FrontRightLeg,
            RearLeftLeg,
            RearRightLeg,
            Torso
        }

        public Animal(GameScene gameScene, Texture2D sprites, Rectangle rightFacing, Rectangle leftFacing) 
            : base(gameScene, sprites, rightFacing, leftFacing)
        {
        }
    }
}
