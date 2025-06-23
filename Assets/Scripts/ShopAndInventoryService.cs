using System.Collections.Generic;
using UnityEngine;

public class ShopAndInventoryService
{
    private ShopController shopController;
    private InventoryController inventoryController;

    public ShopAndInventoryService(List<ItemScriptableObject> itemsSO, ShopView shopPrefab,InventoryView inventoryPrefab, Transform parent)
    {
        CreateControllers(itemsSO,shopPrefab,inventoryPrefab,parent);
    }

    private void CreateControllers(List<ItemScriptableObject> itemsSO, ShopView shopPrefab, InventoryView inventoryPrefab, Transform parent)
    {
        ShopModel shopModel = new ShopModel();
        shopController = new ShopController(itemsSO,shopModel,shopPrefab,parent);

        InventoryModel inventoryModel = new InventoryModel();
        inventoryController = new InventoryController(inventoryModel, inventoryPrefab, parent);
    }
}