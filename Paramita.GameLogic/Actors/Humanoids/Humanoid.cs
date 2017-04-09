using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;

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
        private string[] _equipSlotLabels = new string[] { "left_hand", "right_hand", "head", "body", "feet" };
        private EquipType[] _equipTypes = new EquipType[5] { EquipType.Hand, EquipType.Hand, EquipType.Head, EquipType.Body, EquipType.Feet };
        protected int leftHand = 0;
        protected int rightHand = 1;
        protected int head = 2;
        protected int body = 3;
        protected int feet = 4;
        #endregion


        #region Properties
        public Item LeftHandItem
        {
            get { return Inventory.Equipment[leftHand]; }
        }
        public Item RightHandItem
        {
            get { return Inventory.Equipment[rightHand]; }
        }
        public Item HeadItem
        {
            get { return Inventory.Equipment[head]; }
        }
        public Item BodyItem
        {
            get { return Inventory.Equipment[body]; }
        }
        public Item FeetItem
        {
            get { return Inventory.Equipment[feet]; }
        }
        #endregion


        public Humanoid(BeingType type, List<int> combatData) : base(type, combatData)
        {
            InitializeInventory();
        }

        public void RemoveItem(Item item)
        {
            Discard(item);
        }

        public virtual bool AddItem(Item item)
        {
            return Acquire(item);
        }

        public Item[] InspectItems()
        {
            Item[] items = new Item[10];

            for (int x = 0; x < Inventory.Equipment.Length; x++)
            {
                items[x] = Inventory.Equipment[x];
            }

            for (int x = 0; x < Inventory.Storage.Length; x++)
            {
                items[x + 5] = Inventory.Storage[x];
            }

            return items;
        }


        #region Protected Methods
        protected override void InitializeInventory()
        {
            Inventory.Labels = _equipSlotLabels;
            Inventory.EquipTypes = _equipTypes;
            Inventory.Equipment = new Item[5];
            Inventory.Storage = new Item[5];
        }
        #endregion


        #region Helper Methods
        
        #endregion
    }
}
