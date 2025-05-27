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

    public ShopView GetShopView()//maybe not needed
    {
        return shopView;
    }
    public ShopModel GetShopModel()//maybe not needed
    {
        return shopModel;
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
            ItemModel itemModel = new ItemModel(value.id,
                value.type, value.rarity, value.icon,
                value.description, value.buyingPrice,
                value.sellingPrice, value.weight, value.quantity);

            ItemController itemController = new ItemController(itemModel, shopView.itemButtonPrefab, shopView.itemButtonsParent);

            shopModel.itemsList.Add(itemController);

            itemController.GetItemView().itemButton.onClick.AddListener(() => OnItemButtonClicked(itemModel));
        }
        SetOnlyRequiredTypeVisible(ItemType.Materials);//initially only showing Material Items
    }
    public void SetOnlyRequiredTypeVisible(ItemType _type)
    {
        foreach (var item in shopModel.itemsList)
        {
            if (item.GetItemModel().type == _type)
            {
                item.GetItemView().gameObject.SetActive(true);
            }
            else
                item.GetItemView().gameObject.SetActive(false);
        }
    }

    private void OnItemButtonClicked(ItemModel _model)
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(_model, ItemContext.Buy);//item button clicked from shop will be Buy Action
    }
    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach(var item in shopModel.itemsList)
        {
            if(item.GetItemModel().id == id)
            {
                boughtItem= item;
                break;
            }
        }
        return boughtItem;
    }

    public void OnTransactionPerformed(ItemModel passedModel, ItemContext _context, int _count)
    {
        ItemController tradedItem = GetItemBasedOnId(passedModel.id);
        UpdateChangesOnTradedItem(tradedItem,_context, _count);
        DisplayChangesOnTradedItem(tradedItem,_context,_count);
    }
    private void UpdateChangesOnTradedItem(ItemController tradedItem, ItemContext _context, int _count)
    {
        switch (_context)
        {
            case ItemContext.Buy:
                tradedItem.GetItemModel().quantity = tradedItem.GetItemModel().quantity - _count;
                break;
            case ItemContext.Sell:
                tradedItem.GetItemModel().quantity = tradedItem.GetItemModel().quantity + _count;
                break;
        }
    }
    private void DisplayChangesOnTradedItem(ItemController tradedItem,ItemContext _context, int _count)
    {
        tradedItem.DisplayChangedQuantityText();
    }
}