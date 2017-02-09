using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using System.Collections.Generic;
using Paramita.Levels;

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

        public Animal(Level level, Texture2D sprites) 
            : base(level, sprites)
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
