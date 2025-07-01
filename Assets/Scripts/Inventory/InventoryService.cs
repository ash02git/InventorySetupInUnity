using UnityEngine;
using ShopAndInventory.Item;

namespace ShopAndInventory.Inventory
{
    public class InventoryService
    {
        private InventoryController inventoryController;

        public InventoryService(InventoryView inventoryPrefab, Transform parent)
        {
            InventoryModel inventoryModel = new InventoryModel();
            inventoryController = new InventoryController(inventoryModel, inventoryPrefab, parent);
        }

        public void AssignRandomCurrencyValue() => inventoryController.AssignRandomCurrencyValue();

        public void CreateNewItem(ItemScriptableObject itemSO) => inventoryController.CreateNewItem(itemSO);

        public void UpdateWeight(int cumulativeWeight) => inventoryController.UpdateWeight(cumulativeWeight);

        public int GetMaxWeight() => inventoryController.GetMaxWeight();

        public int GetCurrency() => inventoryController.GetCurrency();

        public int GetCurrentWeight() => inventoryController.GetCurrentWeight();

        //InventoryService can be filled with more methods when development is scaled
        // for example:- if the player picks up an item from environment, it also gets added to inventory
    }
}