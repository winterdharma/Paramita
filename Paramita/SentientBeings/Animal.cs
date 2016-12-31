using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public Animal(Texture2D sprites, Rectangle rightFacing, Rectangle leftFacing) 
            : base(sprites, rightFacing, leftFacing)
        {
        }
    }
}
