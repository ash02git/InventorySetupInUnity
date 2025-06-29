using System.Collections.Generic;
using UnityEngine;
using ShopAndInventory.Item;

namespace ShopAndInventory.Shop
{
    public class ShopService
    {
        private ShopController shopController;

        public ShopService(List<ItemScriptableObject> itemsSO, ShopView shopPrefab, Transform parent)
        {
            ShopModel shopModel = new ShopModel();
            shopController = new ShopController(itemsSO, shopModel, shopPrefab, parent);
        }

        public void MakeItemButtonsInteractable() => shopController.MakeItemButtonsInteractable();

        //ShopService can be filled with more methods when development is scaled
        // for example:- if items in shop are refilled after a particular checkpoint, a function for that can be implemented
    }
}