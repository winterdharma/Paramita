using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Items.Weapons;
using Paramita.Scenes;
using System.Linq;
using System;

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

        public Item LeftHand { get { return leftHand; } }
        public Item RightHand { get { return rightHand; } }
        public Item Head { get { return head; } }
        public Item Body { get { return body; } }
        public Item Feet { get { return feet; } }

        public Humanoid(GameScene gameScene, Texture2D sprites, Rectangle right, Rectangle left) 
            : base(gameScene, sprites, right, left)
        {

        }



        // Checks the EquipType of the @item being equiped, checks to see if there
        // is an open equip slot, and adds the item if the slot can be filled.
        // Returns bool indicating success or failure.
        public override bool EquipItem(Item item)
        {
            bool isEquiped = false;

            // if the item being equiped is a NaturalWeapon, then check if it
            // should be equiped or not
            if(item is NaturalWeapon && item.EquipType != EquipType.None)
            {
                switch(item.EquipType)
                {
                    case EquipType.Hand:
                        if( rightHand == null && 
                            (!(leftHand is Weapon) || leftHand == null) )
                        {
                            isEquiped = true;
                            rightHand = item;
                            return isEquiped;
                        }
                        if(!(rightHand is Weapon) && leftHand == null)
                        {
                            isEquiped = true;
                            leftHand = item;
                            return isEquiped;
                        }
                        return isEquiped;
                }
            }

            // handle the item according to it's EquipType
            switch(item.EquipType)
            {
                case EquipType.None:
                    return isEquiped;

                case EquipType.Hand:
                    if (rightHand == null ||
                        item is Weapon && rightHand is NaturalWeapon)
                    {
                        isEquiped = true;
                        rightHand = item;
                    }
                    else if (leftHand == null || 
                        item is Weapon && leftHand is NaturalWeapon)
                    {
                        isEquiped = true;
                        leftHand = item;
                    }
                    break;

                case EquipType.Body:
                    if (body == null)
                    {
                        isEquiped = true;
                        body = item;
                    }
                    break;

                case EquipType.Head:
                    if (head == null)
                    {
                        isEquiped = true;
                        head = item;
                    }
                    break;

                case EquipType.Feet:
                    if (feet == null)
                    {
                        isEquiped = true;
                        feet = item;
                    }
                    break;
            }

            // update the humanoid's Attacks and Shields lists in case the
            // change has made them out of date
            if(isEquiped == true)
            {
                UpdateAttacks();
                UpdateShields();
            }

            return isEquiped;
        }



        // Checks the being's equip items for Weapons or NaturalWeapons
        public override void UpdateAttacks()
        {
            // reset the attacks list
            attacks.Clear();

            // check for bonus attack NaturalWeapons
            for(int x = 0; x < naturalWeapons.Count; x++)
            {
                if(naturalWeapons[x].EquipType == EquipType.None)
                {
                    attacks.Add(naturalWeapons[x]);
                }
            }

            // next, check for Weapons among being's equiped items
            if (leftHand is Weapon)
                attacks.Add(leftHand as Weapon);
            if (rightHand is Weapon)
                attacks.Add(rightHand as Weapon);
            if (head is Weapon)
                attacks.Add(head as Weapon);
            if (body is Weapon)
                attacks.Add(body as Weapon);
            if (feet is Weapon)
                attacks.Add(feet as Weapon);

            attacks = attacks.OrderBy(w => w.Length).ToList();
            Console.WriteLine(attacks.ToString());
        }



        public override void UpdateShields()
        {
            shields.Clear();

            if (leftHand is Shield)
                shields.Add(leftHand as Shield);
            if (rightHand is Shield)
                shields.Add(rightHand as Shield);
        }



        protected override int GetTotalEncumbrance()
        {
            int total = encumbrance;

            total += GetItemEncumbrance(leftHand);
            total += GetItemEncumbrance(rightHand);
            total += GetItemEncumbrance(head);
            total += GetItemEncumbrance(body);
            total += GetItemEncumbrance(feet);

            return total;
        }


        private int GetItemEncumbrance(Item item)
        {
            if(item is Armor)
            {
                var itemAsArmor = item as Armor;
                return itemAsArmor.Encumbrance;
            }
            else if(item is Shield)
            {
                var itemAsShield = item as Shield;
                return itemAsShield.Encumbrance;
            }
            return 0;
        }


        public override string GetDescription()
        {
            return name;
        }
    }
}
