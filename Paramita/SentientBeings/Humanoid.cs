using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.SentientBeings
{

    public enum HumanoidBodyParts
    {
        Head,
        Torso,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }


    /*
     * This is the base class for all human-like bipedal creatures
     * with a head, two arms, two legs, and stand upright.
     * 
     * The class holds the physical configuration related attributes
     * such as body parts and locations equipped items can be placed.
     */
    public class Humanoid : SentientBeing
    {
        // equiped items
        protected Item leftHand;
        protected Item rightHand;
        protected Item head;
        protected Item body;
        protected Item feet;
        protected Item extra1;
        protected Item extra2;

        public Humanoid(Texture2D sprites, Rectangle right, Rectangle left) 
            : base(sprites, right, left)
        {

        }
    }
}
