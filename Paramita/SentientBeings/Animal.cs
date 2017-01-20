using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using System.Linq;
using System.Collections.Generic;

namespace Paramita.SentientBeings
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

    public class Animal : SentientBeing
    {
        // equiped items
        protected int head = 0;
        protected int frontFeet = 1;
        protected int rearFeet = 2;
        protected int body = 3;
        protected int tail = 4;

        public Animal(GameScene gameScene, Texture2D sprites) 
            : base(gameScene, sprites)
        {
        }


        protected override void InitializeAttributes()
        {
            // implemented in child classes
        }


        protected override void InitializeItemLists()
        {
            equipedItems = new Item[5];
            unequipedItems = new Item[1];
        }


        public override List<int> GetLocationForEquipType(EquipType type)
        {
            List<int> locations = new List<int>();
            if (type == EquipType.Body)
                locations.Add(body);
            else if (type == EquipType.Feet)
            {
                locations.Add(frontFeet);
                locations.Add(rearFeet);
            }
            else if (type == EquipType.Head)
                locations.Add(head);

            return locations;
        }


        public override string GetDescription()
        {
            return name;
        }
    }
}
