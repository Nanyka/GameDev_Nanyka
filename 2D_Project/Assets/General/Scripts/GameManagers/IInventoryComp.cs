using System.Collections.Generic;

namespace TheAiAlchemist
{
    public interface IInventoryComp
    {
        public void Withdraw(int productId);
        public bool IsProductAvailable(int productId);
        public List<int> GetItems();
        public int GetHighestPriority();
        public bool IsEmpty();
        public void ResetInventory();
    }
}