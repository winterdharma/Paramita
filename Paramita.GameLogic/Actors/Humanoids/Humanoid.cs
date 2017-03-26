using Paramita.GameLogic.Items;
using System.Collections.Generic;

namespace Paramita.GameLogic.Actors
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
    public abstract class Humanoid : Actor
    {
        #region Fields
        // @equipedItems index values
        protected int leftHand = 0;
        protected int rightHand = 1;
        protected int head = 2;
        protected int body = 3;
        protected int feet = 4;
        #endregion


        #region Properties
        public Item LeftHandItem
        {
            get { return _equippedItems[leftHand]; }
        }
        public Item RightHandItem
        {
            get { return _equippedItems[rightHand]; }
        }
        public Item HeadItem
        {
            get { return _equippedItems[head]; }
        }
        public Item BodyItem
        {
            get { return _equippedItems[body]; }
        }
        public Item FeetItem
        {
            get { return _equippedItems[feet]; }
        }
        #endregion


        public Humanoid(BeingType type) : base(type)
        {

        }


        public override List<int> GetLocationForEquipType(EquipType type)
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
            DiscardItem(item);
        }

        public virtual bool AddItem(Item item)
        {
            return AcquireItem(item);
        }

        public Item[] InspectItems()
        {
            Item[] items = new Item[10];

            for (int x = 0; x < _equippedItems.Length; x++)
            {
                items[x] = _equippedItems[x];
            }

            for (int x = 0; x < _unequippedItems.Length; x++)
            {
                items[x + 5] = _unequippedItems[x];
            }

            return items;
        }


        #region Protected Methods
        protected override void InitializeAttributes()
        {
            // implemented in child classes
        }


        protected override void InitializeItemLists()
        {
            _equippedItems = new Item[5];
            _unequippedItems = new Item[5];
        }
        #endregion


        #region Helper Methods
        
        #endregion
    }
}
