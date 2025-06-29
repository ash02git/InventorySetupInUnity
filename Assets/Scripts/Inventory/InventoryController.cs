using System.Collections.Generic;
using UnityEngine;
public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    private readonly int lowerCurrencyLimit = 500;
    private readonly int upperCurrencyLimit = 1000;

    public InventoryController(InventoryModel _model, InventoryView prefab, Transform parent)
    {
        inventoryModel = _model;
        inventoryView = GameObject.Instantiate<InventoryView>(prefab, parent);

        inventoryView.SetInventoryController(this);

        GameService.Instance.EventService.OnItemSold.AddListener(OnItemSold);
        GameService.Instance.EventService.OnItemBought.AddListener(OnItemBought);
    }

    public int GetCurrency() => inventoryModel.GetCurrency();

    public int GetMaxWeight() => inventoryModel.GetMaxWeight();

    public int GetCurrentWeight() => inventoryModel.GetCurrentWeight();

    public List<ItemController> GetItemsList() => inventoryModel.GetItemsList();

    public void UpdateWeight(int value)
    {
        inventoryModel.UpdateWeight(value);
        inventoryView.UpdateWeightText();
    }

    public void RemoveItemToInventory(ItemController toBeDeleted) => inventoryModel.RemoveItem(toBeDeleted);

    public void AddItemToInventory(ItemController toBeAdded) => inventoryModel.AddItem(toBeAdded);

    public void AssignRandomCurrencyValue()
    {
        inventoryModel.UpdateCurrency(UnityEngine.Random.Range(lowerCurrencyLimit, upperCurrencyLimit));
        inventoryView.UpdateCurrencyText();
    }

    private void OnItemButtonClicked(ItemScriptableObject itemSO) => GameService.Instance.EventService.OnItemSelected.InvokeEvent(itemSO);

    private void OnItemBought(ItemScriptableObject passedItemSO, int passedCount)
    {
        ItemController boughtItem = GetItemBasedOnId(passedItemSO.id);

        if(boughtItem!=null)
            UpdateCountOfItem(boughtItem, passedCount, passedItemSO.doableAction);
        else
            boughtItem = CreateNewItem(passedItemSO, passedCount);

        UpdateCurrency(boughtItem, passedCount, passedItemSO.doableAction);
        UpdateWeight(boughtItem, passedCount, passedItemSO.doableAction);
    }

    private void OnItemSold(ItemScriptableObject passeditemSO, int passedCount)
    {
        ItemController soldItem = GetItemBasedOnId(passeditemSO.id);

        if(soldItem != null)
        {
            //updating details when item is sold
            UpdateCountOfItem(soldItem, passedCount, passeditemSO.doableAction);
            UpdateCurrency(soldItem, passedCount, passeditemSO.doableAction);
            UpdateWeight(soldItem, passedCount, passeditemSO.doableAction);

            //deleting item if all items are sold
            if (soldItem.HasNoItems())
                DeleteItem(soldItem);
        }
    }

    private void UpdateCountOfItem(ItemController boughtItem,int _count,DoableAction itemPlace) => boughtItem.UpdateInventoryItemQuantity(_count, itemPlace);

    private void UpdateCurrency(ItemController _controller, int _count, DoableAction action) //ItemContext _context
    {
        int changeInCurrency=0;
        if(action == DoableAction.Buy)
            changeInCurrency = -(_controller.GetBuyingPrice() * _count);
        else
            changeInCurrency = +(_controller.GetSellingPrice() * _count);

        inventoryModel.UpdateCurrency(changeInCurrency);
        inventoryView.UpdateCurrencyText();
    }

    private void UpdateWeight(ItemController _controller, int _count, DoableAction action)
    {
        int changeInWeight = 0;
        if (action == DoableAction.Buy)
            changeInWeight = +(_controller.GetWeight() * _count);
        else
            changeInWeight = -(_controller.GetWeight() * _count);

        inventoryModel.UpdateWeight(changeInWeight);
        inventoryView.UpdateWeightText();
    }
    
    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach (var item in GetItemsList() )
        {
            if (item.GetID() == id)
            {
                boughtItem = item;
                break;
            }
        }
        return boughtItem;
    }

    private ItemController CreateNewItem(ItemScriptableObject _itemSO, int _count)
    {
        ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(_itemSO), newItemSO);

        newItemSO.quantity = _count;
        newItemSO.doableAction = DoableAction.Sell;

        ItemController itemController = new ItemController(newItemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);
        AddItemToInventory(itemController);
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(newItemSO));

        return itemController;
    }

    public void CreateNewItem(ItemScriptableObject itemSO)
    {
        ItemController itemController = new ItemController(itemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);
        AddItemToInventory(itemController);
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(itemSO));
    }

    private void DeleteItem(ItemController toBeDeleted) => RemoveItemToInventory(toBeDeleted);
}