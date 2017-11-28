using Paramita.GameLogic.Items;
using Paramita.GameLogic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Paramita.GameLogic.Actors
{
    public class InventoryChangeEventArgs : EventArgs
    {
        public Tuple<Dictionary<string, ItemType>, int> Inventory { get; }

        public InventoryChangeEventArgs(Tuple<Dictionary<string, ItemType>, int> inventory)
        {
            Inventory = inventory;
        }
    }

    public class WeaponsEventArgs : EventArgs
    {
        public List<Weapon> Weapons { get; }

        public WeaponsEventArgs(List<Weapon> weapons)
        {
            Weapons = weapons;
        }
    }

    public class ShieldsEventArgs : EventArgs
    {
        public List<Shield> Shields { get; }

        public ShieldsEventArgs(List<Shield> shields)
        {
            Shields = shields;
        }
    }

    public class Inventory
    {
        #region Fields
        private Item[] _equippedItems;
        private EquipType[] _equipTypes;
        private string[] _equipLabels;
        private Item[] _storedItems;
        private List<Weapon> _weapons = new List<Weapon>();
        private List<NaturalWeapon> _naturalWeapons = new List<NaturalWeapon>();
        private List<Shield> _shields = new List<Shield>();
        private int _gold = 0;
        #endregion

        public event EventHandler<InventoryChangeEventArgs> OnInventoryChange;
        public event EventHandler<WeaponsEventArgs> OnWeaponsChange;
        public event EventHandler<ShieldsEventArgs> OnShieldsChange;
        public event EventHandler<EventData<int>> OnItemEncumbranceChange;

        #region Constructors
        public Inventory() { }

        public Inventory(int equippedSlots, int storageSlots, EquipType[] equipTypes, string[] equipLabels, int gold = 0)
        {
            _equipLabels = equipLabels;
            _equippedItems = new Item[equippedSlots];
            _equipTypes = equipTypes;
            _storedItems = new Item[storageSlots];
            _gold = gold;
        }
        #endregion

        #region Properties
        public string[] Labels { set { _equipLabels = value; } }

        public EquipType[] EquipTypes { set { _equipTypes = value; } }

        public Item[] Equipment
        {
            get { return _equippedItems; }
            set
            {
                _equippedItems = value;
                UpdateWeapons();
                UpdateShields();
                RaiseChangeEvent();
            }
        }
        public Item[] Storage
        {
            get { return _storedItems; }
            set
            {
                _storedItems = value;
                RaiseChangeEvent();
            }
        }

        public List<Weapon> Weapons
        {
            get { return _weapons; }
            set
            {
                _weapons = value;
                OnWeaponsChange?.Invoke(this, new WeaponsEventArgs(Weapons));
            }
        }

        public List<NaturalWeapon> NaturalWeapons
        {
            get { return _naturalWeapons; }
            set { _naturalWeapons = value; }
        }

        public List<Shield> Shields
        {
            get { return _shields; }
            set
            {
                _shields = value;
                OnShieldsChange?.Invoke(this, new ShieldsEventArgs(Shields));
            }
        }
        #endregion


        public bool AddItemToEquipment(Item item)
        {
            if (item.EquipType == EquipType.None)
                return false;

            var validAt = GetSlotsMatching(item.EquipType);
            var equippedItems = Equipment;
            foreach (var index in validAt)
            {

                if ((item is Weapon || item is Shield) && !(item is NaturalWeapon)
                    && equippedItems[index] is NaturalWeapon)
                {
                    equippedItems[index] = item;
                    Equipment = equippedItems;
                    return true;
                }

                if (equippedItems[index] == null)
                {
                    equippedItems[index] = item;
                    Equipment = equippedItems;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItemFromEquipment(Item item)
        {
            int foundAt;
            if (GetSlotItemIsIn(false, item, out foundAt))
            {
                var equippedItems = Equipment;
                equippedItems[foundAt] = null;
                Equipment = equippedItems;
                return true;
            }
            return false;
        }

        public bool AddItemToStorage(Item item)
        {
            var unequippedItems = Storage;
            var emptyAt = new List<int>();
            if (GetEmptySlots(true, out emptyAt))
            {
                unequippedItems[emptyAt[0]] = item;
                Storage = unequippedItems;
                return true;
            }
            else
                return false;
        }

        public bool RemoveItemFromStorage(Item item)
        {
            int foundAt;
            if (GetSlotItemIsIn(true, item, out foundAt))
            {
                var unequippedItems = Storage;
                unequippedItems[foundAt] = null;
                Storage = unequippedItems;
                return true;
            }
            else
                return false;
        }

        public void AddToWeapons(List<NaturalWeapon> weaponList)
        {
            var weapons = Weapons;
            weapons.AddRange(weaponList);
            Weapons = weapons;
        }

        public void RaiseChangeEvent()
        {
            OnInventoryChange?.Invoke(this, new InventoryChangeEventArgs(GetInventoryData()));
            OnItemEncumbranceChange?.Invoke(this, new EventData<int>(GetSumOfItemEncumbrance()));
        }

        #region Helper Methods
        private Tuple<Dictionary<string, ItemType>, int> GetInventoryData()
        {
            var inventoryData = new Dictionary<string, ItemType>();

            Item item;
            string slot;
            for (int i = 0; i < _equippedItems.Length; i++)
            {
                item = _equippedItems[i];
                slot = _equipLabels[i];

                inventoryData[slot] = item != null ? item.ItemType : ItemType.None;
            }


            int num = 0;
            for (int i = 0; i < _storedItems.Length; i++)
            {
                num++;
                slot = "other" + num;
                item = _storedItems[i];
                inventoryData[slot] = item != null ? item.ItemType : ItemType.None;
            }

            return new Tuple<Dictionary<string, ItemType>, int>(inventoryData, _gold);
        }

        private void UpdateWeapons()
        {
            // reset the attacks list
            _weapons.Clear();

            // check for default natural weapons to add if slot is empty
            for (int x = 0; x < _naturalWeapons.Count; x++)
            {
                if (_naturalWeapons[x].EquipType == EquipType.None)
                {
                    _weapons.Add(_naturalWeapons[x]);
                }
                else
                {
                    var slots = GetEmptySlotsMatching(_naturalWeapons[x].EquipType);
                    if (slots.Count > 0)
                        _equippedItems[slots[0]] = _naturalWeapons[x];
                }
            }
            // add all valid weapons equipped to weapons list
            for (int x = 0; x < _equippedItems.Length; x++)
            {
                if (_equippedItems[x] is Weapon)
                {
                    _weapons.Add(_equippedItems[x] as Weapon);
                }
            }

            Weapons = _weapons.OrderBy(w => w.Length).ToList();

        }

        private void UpdateShields()
        {
            var shields = new List<Shield>();

            for (int x = 0; x < _equippedItems.Length; x++)
            {
                if (_equippedItems[x] is Shield)
                {
                    shields.Add(_equippedItems[x] as Shield);
                }
            }
            Shields = shields;
        }

        private bool GetEmptySlots(bool isStorage, out List<int> emptyAt)
        {
            emptyAt = new List<int>();
            Item[] slots;

            if (isStorage)
                slots = Storage;
            else
                slots = Equipment;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    emptyAt.Add(i);
                    return true;
                }
            }
            return false;
        }

        private List<int> GetEmptySlotsMatching(EquipType type)
        {
            var indices = new List<int>();
            for (int i = 0; i < _equipTypes.Length; i++)
            {
                if (_equipTypes[i] == type && _equippedItems[i] == null)
                    indices.Add(i);
            }
            return indices;
        }

        private bool GetSlotItemIsIn(bool isStorage, Item item, out int foundAt)
        {
            Item[] slots;
            if (isStorage)
                slots = Storage;
            else
                slots = Equipment;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == item)
                {
                    foundAt = i;
                    return true;
                }
            }
            foundAt = 1000;
            return false;
        }

        private List<int> GetSlotsMatching(EquipType type)
        {
            var indices = new List<int>();
            for (int i = 0; i < _equipTypes.Length; i++)
            {
                if (_equipTypes[i] == type)
                    indices.Add(i);
            }
            return indices;
        }

        private int GetSumOfItemEncumbrance()
        {
            int total = 0;
            var allItems = new List<Item>();
            allItems.AddRange(Equipment);
            if(Storage != null)
                allItems.AddRange(Storage);

            foreach (var item in allItems)
            {

                total = item != null ? total + item.Encumbrance : total;
            }

            return total;
        }
        #endregion
    }
}
