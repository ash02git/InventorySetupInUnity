using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController
{
    //Dependency
    private ItemDetailsController itemDetailsController;

    //references to model and view
    private ShopModel shopModel;
    private ShopView shopView;

    public ShopController(List<ItemScriptableObject> itemsSO, ShopModel _model, ShopView shopViewPrefab, Transform parent)
    {
        shopModel = _model;
        shopView = GameObject.Instantiate<ShopView>(shopViewPrefab, parent);

        shopModel.SetShopController(this);
        shopView.SetShopController(this);

        CreateShop(itemsSO);
    }

    public void Init(ItemDetailsController itemDetailsController)//Injecting dependency
    {
        this.itemDetailsController = itemDetailsController;
    }

    private void CreateShop(List<ItemScriptableObject> itemsSO)
    {
        CreateItemTypeTabs();
        CreateItemButtons(itemsSO);
    }

    private void CreateItemTypeTabs()
    {
        foreach (ItemType value in Enum.GetValues(typeof(ItemType)))
        {
            Button tabButton = GameObject.Instantiate<Button>(shopView.itemTypeTabPrefab, shopView.itemTypeTabsParent);
            tabButton.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
            tabButton.onClick.AddListener(() => SetOnlyRequiredTypeVisible(value));
        }
    }

    private void CreateItemButtons(List<ItemScriptableObject> itemsSO)
    {
        foreach (ItemScriptableObject value in itemsSO)
        {
            ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(value), newItemSO);

                Debug.Log(newItemSO.id + " created");
                Debug.Log(newItemSO.id + " quantity = " +  newItemSO.quantity);

            ItemController itemController = new ItemController(newItemSO, shopView.itemButtonPrefab, shopView.itemButtonsParent);
            //itemController.GetItemView().itemButton.interactable = false;  
            itemController.SetItemButtonNonInteractable();

            shopModel.itemsList.Add(itemController);

            itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(newItemSO));//previously first parameter was ItemModel
        }
        SetOnlyRequiredTypeVisible(ItemType.Materials);//initially only showing Material Items
    }
    public void SetOnlyRequiredTypeVisible(ItemType _type)
    {
        foreach (var item in shopModel.itemsList)
        {
            if (item.GetItemType() == _type)//previous condition - item.GetItemModel().type == _type
            {
                //item.GetItemView().gameObject.SetActive(true);
                item.ShowItemButton();
            }
            else
            {
                //item.GetItemView().gameObject.SetActive(false);
                item.HideItemButton();
            }
        }
    }

    private void OnItemButtonClicked(ItemScriptableObject itemSO)//previously first parameter was ItemModel
    {
        //if(itemSO.id == ItemID.Feather)
        //{
        //    Debug.Log("Feather is clicked");
        //    Debug.Log("Feather quantity = " + itemSO.quantity);
        //}
        
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(itemSO, ItemContext.Buy);//previously first parameter was ItemModel
    }
    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach(var item in shopModel.itemsList)
        {
            if(item.GetID() == id)//previous code - item.GetItemModel().id == id
            {
                boughtItem= item;
                break;
            }
        }
        return boughtItem;
    }

    public void OnTransactionPerformed(ItemScriptableObject passedItemSO, ItemContext passedContext, int passedCount)//previously first parameter was ItemModel
    {
        ItemController tradedItem = GetItemBasedOnId(passedItemSO.id);
        UpdateChangesOnTradedItem(tradedItem,passedContext, passedCount);
        DisplayChangesOnTradedItem(tradedItem);//, _context);//,_count);

        if (passedItemSO.id == ItemID.Feather)
        {
            Debug.Log("Feather has been " + passedContext);
            Debug.Log("Quantity is " + passedCount);
        }
    }
    private void UpdateChangesOnTradedItem(ItemController tradedItem, ItemContext _context, int _count)
    {
        //switch (_context)
        //{
        //    case ItemContext.Buy:
        //        //tradedItem.GetItemModel().quantity = tradedItem.GetItemModel().quantity - _count;
        //        tradedItem.UpdateShopItemQuantity(_count);
        //        break;
        //    case ItemContext.Sell:
        //        //tradedItem.GetItemModel().quantity = tradedItem.GetItemModel().quantity + _count;
        //        tradedItem.UpdateInventoryItemQuantity(_count);
        //        break;
        //}
        Debug.Log("Updating changes on feather");
        tradedItem.UpdateShopItemQuantity(_count, _context);////this single line will take care of updating count of item in shop
    }
    private void DisplayChangesOnTradedItem(ItemController tradedItem)//,ItemContext _context)//, int _count)
    {
        tradedItem.DisplayChangedQuantityText();
    }

    public void TurnOnAllButtons()
    {
        foreach(ItemController item in shopModel.itemsList)
        {
            //item.GetItemView().itemButton.interactable = true;
            item.SetItemButtonInteractable();
        }
    }
}