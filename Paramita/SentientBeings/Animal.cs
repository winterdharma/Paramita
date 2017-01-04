using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using System.Linq;

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
        protected Item head;
        protected Item frontFeet;
        protected Item rearFeet;
        protected Item body;
        protected Item tail;

        public Animal(GameScene gameScene, Texture2D sprites, Rectangle rightFacing, Rectangle leftFacing) 
            : base(gameScene, sprites, rightFacing, leftFacing)
        {
        }

        public override string GetDescription()
        {
            return name;
        }

        public override bool EquipItem(Item item)
        {
            bool isEquiped = false;

            // if the item being equiped is a NaturalWeapon, then check if it
            // should be equiped or not
            if (item is NaturalWeapon && item.EquipType != EquipType.None)
            {
                switch (item.EquipType)
                {
                    case EquipType.Head:
                        if (head == null)
                        {
                            isEquiped = true;
                            head = item;
                        }
                        break;
                    case EquipType.Feet:
                        if(frontFeet == null)
                        {
                            isEquiped = true;
                            frontFeet = item;
                            break;
                        }
                        if(rearFeet == null)
                        {
                            isEquiped = true;
                            rearFeet = item;
                        }
                        break;
                }
            }

            // handle the item according to it's EquipType
            switch (item.EquipType)
            {
                case EquipType.None:
                    return isEquiped;

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
                    if (frontFeet == null)
                    {
                        isEquiped = true;
                        frontFeet = item;
                        break;
                    }
                    if (rearFeet == null)
                    {
                        isEquiped = true;
                        rearFeet = item;
                    }
                    break;
            }

            // refresh the being's attack list if his equipment has changed
            // (it's possible that a default weapon was removed by equiping
            //  a non-weapon item, so don't check if a weapon was equiped)
            if (isEquiped == true)
                UpdateAttacks();

            return isEquiped;
        }

        public override void UpdateAttacks()
        {
            // reset the attacks list
            attacks.Clear();

            // check for bonus attack NaturalWeapons
            for (int x = 0; x < naturalWeapons.Count; x++)
            {
                if (naturalWeapons[x].EquipType == EquipType.None)
                {
                    attacks.Add(naturalWeapons[x]);
                }
            }

            // next, check for Weapons among being's equiped items
            if (head is Weapon)
                attacks.Add(head as Weapon);
            if (frontFeet is Weapon)
                attacks.Add(frontFeet as Weapon);
            if (rearFeet is Weapon)
                attacks.Add(rearFeet as Weapon);
            if (body is Weapon)
                attacks.Add(body as Weapon);
            if (tail is Weapon)
                attacks.Add(tail as Weapon);

            attacks = attacks.OrderBy(w => w.Length).ToList();
            System.Console.WriteLine(attacks.ToString());
        }
    }
}
