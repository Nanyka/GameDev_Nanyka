using System.Collections.Generic;

namespace TheAiAlchemist
{
    public interface IInventoryComp
    {
        public void Withdraw(int productId);
        public bool IsProductAvailable(int productId);
        public List<int> GetInventory();
        public void ResetInventory();
    }
}