using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class Inventory : ICloneable
    {
        private Dictionary<int, int> _inventory;

        public Inventory(Dictionary<int, int> initialInventory = null)
        {
            if (initialInventory == null)
            {
                _inventory = new Dictionary<int, int> { { 1, 2 }, { 2, 2 }, { 3, 2 } };
            }
            else
            {
                _inventory = new Dictionary<int, int>(initialInventory);
            }
        }

        public void Deduct(int strength)
        {
            if (!_inventory.ContainsKey(strength))
            {
                Debug.LogWarning($"Inventory: Tried to deduct invalid strength key: {strength}.");
                return;
            }

            if (_inventory[strength] > 0)
            {
                _inventory[strength] -= 1;
            }
            else
            {
                Debug.LogWarning($"Inventory: Tried to deduct strength {strength}, but none available.");
            }
        }

        public void Add(int strength)
        {
            if (!_inventory.ContainsKey(strength))
            {
                Debug.LogWarning($"Inventory: Attempting to add invalid strength key: {strength}.");
                return;
            }

            _inventory[strength]++;
        }

        public bool Enough(int strength)
        {
            if (!_inventory.ContainsKey(strength)) return false;
            return _inventory[strength] > 0;
        }

        public List<int> GetValidInventoryStrengths()
        {
            return _inventory.Where(pair => pair.Value > 0).Select(pair => pair.Key).ToList();
        }

        public int GetCount(int strength)
        {
            if (!_inventory.ContainsKey(strength))
            {
                Debug.LogWarning($"Inventory: Tried to get count for invalid strength key: {strength}. Returning 0.");
                return 0;
            }

            return _inventory[strength];
        }

        /// <summary>
        /// Creates a deep copy of the inventory. Implements ICloneable.
        /// </summary>
        /// <returns>An object that is a deep copy of the current instance.</returns>
        public object Clone()
        {
            // Create a new Inventory object using a copy of the internal dictionary
            return new Inventory(new Dictionary<int, int>(this._inventory));
        }


        // Add this method back, as it's still a clean way to get the underlying data if needed elsewhere,
        // even if ApplyMove uses ICloneable now.
        public Dictionary<int, int> GetInventoryDictionary()
        {
            return new Dictionary<int, int>(_inventory); // Return a copy
        }


        public override string ToString()
        {
            var items = string.Join(", ", _inventory.Select(pair => $"{pair.Key}: {pair.Value}"));
            return $"Inventory({{{items}}})";
        }
    }
}