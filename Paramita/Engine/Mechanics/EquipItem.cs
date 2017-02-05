using Paramita.Items;
using Paramita.SentientBeings;
using System.Collections.Generic;

namespace Paramita.Mechanics
{
    public class EquipItem
    {
        private SentientBeing equippingBeing;
        private Item itemToEquip;
        private bool isEquipped;
        private List<int> locations;


        public bool IsEquipped { get { return isEquipped; } }




        public EquipItem(SentientBeing being, Item item)
        {
            equippingBeing = being;
            itemToEquip = item;
            locations = equippingBeing.GetLocationForEquipType(itemToEquip.EquipType);
            isEquipped = TryToEquipItem();
        }




        public bool TryToEquipItem()
        {
            if (itemToEquip.EquipType == EquipType.None)
                return false;


            if (itemToEquip is NaturalWeapon)
            {
                return EquipNaturalWeapon();
            }
            else if (itemToEquip is Weapon)
            {
                isEquipped = EquipWeapon();
            }
            else if (itemToEquip is Shield)
            {
                isEquipped = EquipShield();
            }
            else
            {
                isEquipped = EquipOtherItem();
            }


            if (isEquipped)
            {
                equippingBeing.UpdateAttacks();
            }

            return isEquipped;
        }


        protected bool EquipNaturalWeapon()
        {
            if (IsWeaponEquipedAtLocations())
                return false;
            if (TryToEquipItemAtLocations())
                return true;

            return false;
        }


        protected bool IsWeaponEquipedAtLocations()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                int location = locations[i];
                if (equippingBeing.IsWeaponEquippedAt(location))
                    return true;
            }
            return false;
        }


        protected bool TryToEquipItemAtLocations()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                int location = locations[i];
                if (equippingBeing.IsEquipmentSlotEmpty(location))
                {
                    EquipItemAt(location);
                    return true;
                }
            }
            return false;
        }


        protected void EquipItemAt(int location)
        {
            equippingBeing.EquipmentSlots[location] = itemToEquip;
        }


        protected bool EquipWeapon()
        {
            if (TryToReplaceNaturalWeaponAtLocations() || TryToEquipItemAtLocations())
                return true;
            return false;
        }

        
        private bool TryToReplaceNaturalWeaponAtLocations()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                int location = locations[i];
                if (equippingBeing.IsNaturalWeaponEquipedAt(location))
                {
                    equippingBeing.EquipmentSlots[location] = itemToEquip;
                    return true;
                }
            }
            return false;
        }


        private bool EquipShield()
        {
            if (TryToEquipItemAtLocations() || TryToReplaceNaturalWeaponAtLocations())
            {
                equippingBeing.UpdateShields();
                return true;
            }
            return false;
        }


        protected bool EquipOtherItem()
        {
            if (TryToEquipItemAtLocations())
                return true;
            return false;
        }
    }
}
