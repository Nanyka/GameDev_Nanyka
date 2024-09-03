using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheAiAlchemist
{
    public class InventoryComp : MonoBehaviour, IInventoryComp
    {
        private List<int> _inventory;

        public void Withdraw(int productId)
        {
            _inventory[productId] -= _inventory.Count < productId + 1? 0 : 1;
        }

        public bool IsProductAvailable(int productId)
        {
            if (_inventory.Count < productId + 1)
                return false;

            return _inventory[productId] > 0;
        }

        public List<int> GetItems()
        {
            return _inventory;
        }

        public bool IsEmpty()
        {
            return _inventory.Sum() <= 0;
        }

        public void ResetInventory()
        {
            _inventory = new List<int> { 2, 2, 2 };
        }
    }
}