using UnityEngine;
using static UnityEditor.Progress;
public class InventoryController
{
    //Dependency
    private ItemDetailsController itemDetailsController;

    //references to model and view
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    public InventoryController(InventoryModel _model, InventoryView prefab, Transform parent)
    {
        inventoryModel = _model;
        inventoryView = GameObject.Instantiate<InventoryView>(prefab, parent);

        inventoryModel.SetInventoryController(this);
        inventoryView.SetInventoryController(this);
    }

    public void Init(ItemDetailsController itemDetailsController)
    {
        this.itemDetailsController = itemDetailsController;
    }
    
    public InventoryModel GetInventoryModel()
    {
        return inventoryModel;
    }
    public InventoryView GetInventoryView()
    {
        return inventoryView;
    }
    public void AssignRandomCurrencyValue()
    {
        inventoryModel.UpdateCurrency(UnityEngine.Random.Range(500, 1000));//hardcoded for now
        inventoryView.UpdateCurrencyText();
    }
    private void OnItemButtonClicked(ItemScriptableObject itemSO)//previosly parameter was ItemModel
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(itemSO, ItemContext.Sell);//item button clicked from shop will be Buy Action//previously, first paramter was ItemModel
    }

    public void OnTransactionPerformed(ItemScriptableObject passedItemSO, ItemContext passedContext, int passedCount)//previously first parameter was ItemModel
    {
        switch (passedContext)
        {
            case ItemContext.Sell:
                OnItemSold(passedItemSO, passedCount);//previously first parameter was ItemModel
                break;
            case ItemContext.Buy:
                OnItemBought(passedItemSO, passedCount);//previously first parameter was ItemModel
                break;
        }

        if (passedItemSO.id == ItemID.Feather)
        {
            Debug.Log("Feather has been " + passedContext);
            Debug.Log("Quantity is " + passedCount);
        }
    }

    public void OnItemBought(ItemScriptableObject passedItemSO, int passedCount)//previously first parameter was ItemModel
    {
        ItemController boughtItem = GetItemBasedOnId(passedItemSO.id);

        if(boughtItem!=null)
            UpdateCountOfItem(boughtItem, passedCount, ItemContext.Buy);
        else
            boughtItem = CreateNewItem(passedItemSO, passedCount);

        UpdateCurrency(boughtItem, passedCount, ItemContext.Buy);
        UpdateWeight(boughtItem, passedCount, ItemContext.Buy);
    }

    public void OnItemSold(ItemScriptableObject passeditemSO, int passedCount)//previously first parameter was ItemModel
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

    private void UpdateCountOfItem(ItemController boughtItem,int _count,ItemContext _context)
    {
        //if (_context == ItemContext.Buy)
        //    //boughtItem.GetItemModel().quantity += _count;//previous code is boughtItem.GetItemModel().quantity += _count;
        //    boughtItem.UpdateInventoryItemQuantity(_count,_context);
        //else
        //    //boughtItem.GetItemModel().quantity -= _count;//previous code is boughtItem.GetItemModel().quantity -= _count;
        //    boughtItem.UpdateInventoryItemQuantity(_count);

        boughtItem.UpdateInventoryItemQuantity(_count, _context);//this single line will take care of updating count of item in inventory

        //boughtItem.GetItemView().UpdateQuantityText();
    }

    private void UpdateCurrency(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInCurrency=0;
        if(_context==ItemContext.Buy)
            changeInCurrency = -(_controller.GetBuyingPrice() * _count);//previous code - changeInCurrency = -(_controller.GetItemModel().buyingPrice * _count);
        else
            changeInCurrency = +(_controller.GetSellingPrice() * _count);//previous code - changeInCurrency = +(_controller.GetItemModel().sellingPrice * _count);

        inventoryModel.UpdateCurrency(changeInCurrency);//updated change in currency
        inventoryView.UpdateCurrencyText();
    }

    private void UpdateWeight(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInWeight = 0;
        if (_context == ItemContext.Buy)
            changeInWeight = +(_controller.GetWeight() * _count);//previous code - changeInWeight = +(_controller.GetItemModel().weight * _count);
        else
            changeInWeight = -(_controller.GetWeight() * _count);

        inventoryModel.UpdateWeight(changeInWeight);
        inventoryView.UpdateWeightText();
    }
    
    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach (var item in inventoryModel.itemsList)
        {
            if (item.GetID() == id) //previous condition - item.GetItemModel().id == id
            {
                boughtItem = item;
                break;
            }
        }
        return boughtItem;
    }

    private ItemController CreateNewItem(ItemScriptableObject _itemSO, int _count)//previously first parameter was ItemModel
    {
        //ItemModel itemModel = new ItemModel(_model.id, _model.type, _model.rarity,
        //                                        _model.icon, _model.description, _model.buyingPrice,
        //                                        _model.sellingPrice, _model.weight, _count);

        
        ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(_itemSO), newItemSO);

        newItemSO.quantity = _count;


        ItemController itemController = new ItemController(newItemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);//previously first parameter was ItemModel

        inventoryModel.itemsList.Add(itemController);

        //itemController.GetItemView().itemButton.onClick.AddListener(()=>OnItemButtonClicked(newItemSO));
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(newItemSO));

        return itemController;
    }
    public void CreateNewItem(ItemScriptableObject itemSO)//previously parameter was ItemModel
    {
        ItemController itemController = new ItemController(itemSO, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);//first paramter was ItemModel
        inventoryModel.itemsList.Add(itemController);
        //itemController.GetItemView().itemButton.onClick.AddListener(() => OnItemButtonClicked(itemSO));//previously first parameter was ItemModel
        itemController.GetItemButton().onClick.AddListener(() => OnItemButtonClicked(itemSO));
    }

    private void DeleteItem(ItemController toBeDeleted)
    {
        GetInventoryModel().itemsList.Remove(toBeDeleted);
        toBeDeleted.DestroyItem();
    }
}