using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleRPGShop
{

    class ShopItem
    {
        public enum ItemType
        {
            Weapon,
            Armour,
            Consumable
        }

        public ShopItem(string name, int value, int typeInt)
        {
            _itemName = name;
            _itemValue = value;

            ItemType type = (ItemType)typeInt;

            _type = type; 
        }

        public string GetName()
        {
            return _itemName;
        }

        public int GetValue()
        {
            return _itemValue;
        }

        public ItemType GetType()
        {
            return _type;
        }

        private string _itemName;
        private int _itemValue;
        private ItemType _type;
    }
}
