using System.Collections.Generic;
using ShopAndInventory.Item;

namespace ShopAndInventory.Inventory
{
    public class InventoryModel
    {
        //list of all items in Inventory
        private List<ItemController> itemsList;

        //Data specific to inventory
        private int currency;
        private int maxWeight;
        private int currentWeight;

        public InventoryModel()
        {
            itemsList = new List<ItemController>();
            currency = 0;
            maxWeight = 1000;
            currentWeight = 0;
        }

        public int GetCurrency() => currency;

        public int GetMaxWeight() => maxWeight;

        public int GetCurrentWeight() => currentWeight;

        public List<ItemController> GetItemsList() => itemsList;

        public void UpdateCurrency(int value) => currency += value;

        public void UpdateWeight(int value) => currentWeight += value;

        public void AddItem(ItemController toBeAdded) => itemsList.Add(toBeAdded);

        public void RemoveItem(ItemController toBeDeleted)
        {
            itemsList.Remove(toBeDeleted);
            toBeDeleted.DestroyItem();
        }
    }
}