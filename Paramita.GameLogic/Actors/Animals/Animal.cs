using Paramita.GameLogic.Items;
using System.Collections.Generic;
using System;

namespace Paramita.GameLogic.Actors
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

    public class Animal : Actor
    {
        // equiped items
        private string[] _equipSlotLabels = new string[] { "head", "front_legs", "rear_legs", "body", "tail" };
        private EquipType[] _equipTypes = new EquipType[5] { EquipType.Head, EquipType.Feet, EquipType.Feet, EquipType.Body, EquipType.Tail };
        protected int head = 0;
        protected int frontFeet = 1;
        protected int rearFeet = 2;
        protected int body = 3;
        protected int tail = 4;

        public Animal(BeingType type, List<int> combatData) : base(type, combatData)
        {
            InitializeInventory();
        }

        protected override void InitializeInventory()
        {
            Inventory.Equipment = new Item[5];
            Inventory.Storage = new Item[1];
            Inventory.Labels = _equipSlotLabels;
            Inventory.EquipTypes = _equipTypes;
        }


        public override string GetDescription()
        {
            return name;
        }
    }
}
