using System.Collections.Generic;
using UnityEngine;
public class InventoryController
{
    //Dependency
    private ItemDetailsController itemDetailsController;

    //references to model and view
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    private readonly int lowerCurrencyLimit=500;
    private readonly int upperCurrencyLimit=1000;

    public InventoryController(InventoryModel _model, InventoryView prefab, Transform parent)
    {
        inventoryModel = _model;
        inventoryView = GameObject.Instantiate<InventoryView>(prefab, parent);

        inventoryView.SetInventoryController(this);
    }

    public void Init(ItemDetailsController itemDetailsController)
    {
        this.itemDetailsController = itemDetailsController;
    }

    public int GetCurrency() => inventoryModel.GetCurrency();

    public int GetMaxWeight() => inventoryModel.GetMaxWeight();
    
    public int GetCurrentWeight() => inventoryModel.GetCurrentWeight();

    public List<ItemController> GetItemsList() => inventoryModel.GetItemsList();

    public void UpdateWeight(int value) => inventoryModel.UpdateWeight(value);

    public void RemoveItem(ItemController toBeDeleted) => inventoryModel.RemoveItem(toBeDeleted);

    public void AddItem(ItemController toBeAdded) => inventoryModel.AddItem(toBeAdded);

    public void UpdateWeightText() => inventoryView.UpdateWeightText();

    public void AssignRandomCurrencyValue()
    {
        inventoryModel.UpdateCurrency(UnityEngine.Random.Range(lowerCurrencyLimit, upperCurrencyLimit));
        inventoryView.UpdateCurrencyText();//maybe change to a function
    }

    private void OnItemButtonClicked(ItemScriptableObject itemSO)
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(itemSO, ItemContext.Sell);
    }

    public void OnTransactionPerformed(ItemScriptableObject passedItemSO, ItemContext passedContext, int passedCount)
    {
        switch (passedContext)
        {
            case ItemContext.Sell:
                OnItemSold(passedItemSO, passedCount);
                break;
            case ItemContext.Buy:
                OnItemBought(passedItemSO, passedCount);
                break;
        }
    }

    public void OnItemBought(ItemScriptableObject passedItemSO, int passedCount)
    {
        ItemController boughtItem = GetItemBasedOnId(passedItemSO.id);

        if(boughtItem!=null)
            UpdateCountOfItem(boughtItem, passedCount, ItemContext.Buy);
        else
            boughtItem = CreateNewItem(passedItemSO, passedCount);

        UpdateCurrency(boughtItem, passedCount, ItemContext.Buy);
        UpdateWeight(boughtItem, passedCount, ItemContext.Buy);
    }

    public void OnItemSold(ItemScriptableObject passeditemSO, int passedCount)
    {
        ItemController soldItem = GetItemBasedOnId(passeditemSO.id);

        if(soldItem != null)
        {
            //updating details when item is sold
            UpdateCountOfItem(soldItem, passedCount, ItemContext.Sell);
            UpdateCurrency(soldItem, passedCount, ItemContext.Sell);
            UpdateWeight(soldItem, passedCount, ItemContext.Sell);

            //deleting item if all items are sold
            if (soldItem.HasNoItems())//previous code is soldItem.GetItemModel().quantity <= 0
                DeleteItem(soldItem);
        }
    }

    private void UpdateCountOfItem(ItemController boughtItem,int _count,ItemContext _context) => boughtItem.UpdateInventoryItemQuantity(_count, _context);

    private void UpdateCurrency(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInCurrency=0;
        if(_context==ItemContext.Buy)
            changeInCurrency = -(_controller.GetBuyingPrice() * _count);
        else
            changeInCurrency = +(_controller.GetSellingPrice() * _count);

        inventoryModel.UpdateCurrency(changeInCurrency);//updated change in currency
        inventoryView.UpdateCurrencyText();
    }

    private void UpdateWeight(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInWeight = 0;
        if (_context == ItemContext.Buy)
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

        ItemController itemController = new ItemController(newItemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);
        AddItem(itemController);
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(newItemSO));

        return itemController;
    }

    public void CreateNewItem(ItemScriptableObject itemSO)
    {
        ItemController itemController = new ItemController(itemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);
        AddItem(itemController);
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(itemSO));
    }

    private void DeleteItem(ItemController toBeDeleted) => RemoveItem(toBeDeleted);
}