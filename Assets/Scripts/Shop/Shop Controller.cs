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

    public ShopController( List<ItemScriptableObject> itemsSO, ShopModel _model, ShopView shopViewPrefab, Transform parent )
    {
        shopModel = _model;
        shopView = GameObject.Instantiate<ShopView>( shopViewPrefab, parent );

        shopView.SetShopController(this);
        CreateShop(itemsSO);

        GameService.Instance.EventService.OnItemBought.AddListener( OnItemBought );
        GameService.Instance.EventService.OnItemSold.AddListener( OnItemSold );
    }
    
    public void Init( ItemDetailsController itemDetailsController ) => this.itemDetailsController = itemDetailsController;

    public List<ItemController> GetItemsList() => shopModel.GetItemsList();

    private void CreateShop( List<ItemScriptableObject> itemsSO )
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

    private void CreateItemButtons( List<ItemScriptableObject> itemsSO )
    {
        foreach ( ItemScriptableObject value in itemsSO )
        {
            ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(value), newItemSO);
            ItemController itemController = new ItemController(newItemSO, shopView.itemButtonPrefab, shopView.itemButtonsParent);

            itemController.SetItemButtonNonInteractable();
            AddItemToShop(itemController);

            itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(newItemSO));
        }

        //initially only showing Material Type Items
        SetOnlyRequiredTypeVisible(ItemType.Materials);
    }

    public void SetOnlyRequiredTypeVisible(ItemType _type)
    {
        foreach (var item in GetItemsList())
        {
            if (item.GetItemType() == _type)
                item.ShowItemButton();
            else
                item.HideItemButton();
        }
    }

    private void OnItemButtonClicked(ItemScriptableObject itemSO)
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(itemSO);
    }

    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach(var item in GetItemsList())
        {
            if(item.GetID() == id)
            {
                boughtItem= item;
                break;
            }
        }
        return boughtItem;
    }

    private void OnItemSold(ItemScriptableObject passeditemSO, int passedCount)
    {
        ItemController soldItem = GetItemBasedOnId(passeditemSO.id);

        if (soldItem != null)
            UpdateCountOfItem(soldItem, passedCount, passeditemSO.doableAction);
    }

    private void OnItemBought(ItemScriptableObject passedItemSO, int passedCount)
    {
        ItemController boughtItem = GetItemBasedOnId(passedItemSO.id);

        if (boughtItem != null)
            UpdateCountOfItem(boughtItem, passedCount, passedItemSO.doableAction);
    }

    private void UpdateCountOfItem(ItemController boughtItem, int _count, DoableAction action) => boughtItem.UpdateShopItemQuantity(_count, action);

    public void MakeItemButtonsInteractable()
    {
        foreach(ItemController item in GetItemsList())
            item.SetItemButtonInteractable();
    }

    private void AddItemToShop(ItemController toBeAdded) => shopModel.AddItem(toBeAdded);
}