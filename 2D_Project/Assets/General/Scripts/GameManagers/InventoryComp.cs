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
            _inventory[productId] -= 1;
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

        public int GetHighestPriority()
        {
            var highestPriority = 0;
            for (var i = 0; i < _inventory.Count; i++)
                if (_inventory[i] > 0)
                    highestPriority = i;
            
            return highestPriority;
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