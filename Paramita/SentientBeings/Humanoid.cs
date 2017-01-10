using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.Scenes;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Paramita.SentientBeings
{

    public enum HumanoidBodyParts
    {
        Head = 0,
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
    public abstract class Humanoid : SentientBeing
    {
        // @equipedItems index values
        protected int leftHand = 0;
        protected int rightHand = 1;
        protected int head = 2;
        protected int body = 3;
        protected int feet = 4;

        public Item LeftHandItem
        {
            get { return equipedItems[leftHand]; }
        }
        public Item RightHandItem
        {
            get { return equipedItems[rightHand]; }
        }
        public Item HeadItem
        {
            get { return equipedItems[head]; }
        }
        public Item BodyItem
        {
            get { return equipedItems[body]; }
        }
        public Item FeetItem
        {
            get { return equipedItems[feet]; }
        }



        public Humanoid(GameScene gameScene, Texture2D sprites, Rectangle right, Rectangle left) 
            : base(gameScene, sprites, right, left)
        {

        }


        protected override void InitializeAttributes()
        {
            // implemented in child classes
        }


        protected override void InitializeItemLists()
        {
            equipedItems = new Item[5];
            unequipedItems = new Item[5];
        }


        protected override List<int> GetLocationForEquipType(EquipType type)
        {
            List<int> locations = new List<int>();
            if (type == EquipType.Hand)
            {
                locations.Add(leftHand);
                locations.Add(rightHand);
            }
            else if (type == EquipType.Body)
                locations.Add(body);
            else if (type == EquipType.Feet)
                locations.Add(feet);
            else if (type == EquipType.Head)
                locations.Add(head);

            return locations;
        }



        public void RemoveItem(Item item)
        {

            for (int x = 0; x < unequipedItems.Length; x++)
            {
                if (unequipedItems[x] != null && unequipedItems[x].Equals(item))
                {
                    unequipedItems[x] = null;
                    return;
                }
            }

            for (int x = 0; x < equipedItems.Length; x++)
            {
                if (equipedItems[x] != null && equipedItems[x].Equals(item))
                {
                    equipedItems[x] = null;
                    TryToReplaceWithNaturalWeapon(item, x);
                }
            }

        }


        private void TryToReplaceWithNaturalWeapon(Item item, int location)
        {
            if ((item is Weapon) == false)
                return;

            var locations = GetLocationForEquipType(item.EquipType);

            if (IsWeaponEquipedAtLocations(locations) == true)
                return;

            for(int x = 0; x < naturalWeapons.Count; x++)
            {
                if(naturalWeapons[x].EquipType == item.EquipType)
                {
                    equipedItems[location] = naturalWeapons[x];
                }
            }
        }
        

        public virtual bool AddItem(Item item)
        {
            bool equiped = TryToEquipItem(item);

            if (equiped == false)
            {
                for (int x = 0; x < unequipedItems.Length; x++)
                {
                    if (unequipedItems[x] == null)
                    {
                        unequipedItems[x] = item;
                        return true;
                    }
                }
                return false;
            }

            return true;
        }


        public Item[] InspectItems()
        {
            Item[] items = new Item[10];

            for (int x = 0; x < equipedItems.Length; x++)
            {
                items[x] = equipedItems[x];
            }

            for (int x = 0; x < unequipedItems.Length; x++)
            {
                items[x + 5] = unequipedItems[x];
            }

            return items;
        }
    }
}
