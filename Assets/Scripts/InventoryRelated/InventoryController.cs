using UnityEngine;
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
    private void OnItemButtonClicked(ItemModel _model)
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(_model, ItemContext.Sell);//item button clicked from shop will be Buy Action
    }

    public void OnTransactionPerformed(ItemModel passedModel, ItemContext passedContext, int passedCount)
    {
        switch (passedContext)
        {
            case ItemContext.Sell:
                OnItemSold(passedModel, passedCount);
                break;
            case ItemContext.Buy:
                OnItemBought(passedModel, passedCount);
                break;
        }
    }

    public void OnItemBought(ItemModel passedModel, int passedCount)
    {
        ItemController boughtItem = GetItemBasedOnId(passedModel.id);

        if(boughtItem!=null)
            UpdateCountOfItem(boughtItem, passedCount, ItemContext.Buy);
        else
            boughtItem = CreateNewItem(passedModel, passedCount);

        UpdateCurrency(boughtItem, passedCount, ItemContext.Buy);
        UpdateWeight(boughtItem, passedCount, ItemContext.Buy);
    }

    public void OnItemSold(ItemModel passedModel, int passedCount)
    {
        ItemController soldItem = GetItemBasedOnId(passedModel.id);

        if(soldItem != null)
        {
            //updating details when item is sold
            UpdateCountOfItem(soldItem, passedCount, ItemContext.Sell);
            UpdateCurrency(soldItem, passedCount, ItemContext.Sell);
            UpdateWeight(soldItem, passedCount, ItemContext.Sell);

            //deleting item if all items are sold
            if (soldItem.GetItemModel().quantity <= 0)
                DeleteItem(soldItem);
        }
    }

    private void UpdateCountOfItem(ItemController boughtItem,int _count,ItemContext _context)
    {
        if(_context==ItemContext.Buy)
            boughtItem.GetItemModel().quantity += _count;
        else
            boughtItem.GetItemModel().quantity -= _count;

        boughtItem.GetItemView().UpdateQuantityText();
    }

    private void UpdateCurrency(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInCurrency=0;
        if(_context==ItemContext.Buy)
            changeInCurrency = -(_controller.GetItemModel().buyingPrice * _count);
        else
            changeInCurrency = +(_controller.GetItemModel().sellingPrice * _count);

        inventoryModel.UpdateCurrency(changeInCurrency);//updated change in currency
        inventoryView.UpdateCurrencyText();
    }

    private void UpdateWeight(ItemController _controller,int _count,ItemContext _context)
    {
        int changeInWeight = 0;
        if (_context == ItemContext.Buy)
            changeInWeight = +(_controller.GetItemModel().weight * _count);
        else
            changeInWeight = -(_controller.GetItemModel().weight * _count);

        inventoryModel.UpdateWeight(changeInWeight);
        inventoryView.UpdateWeightText();
    }
    
    private ItemController GetItemBasedOnId(ItemID id)
    {
        ItemController boughtItem = null;
        foreach (var item in inventoryModel.itemsList)
        {
            if (item.GetItemModel().id == id)
            {
                boughtItem = item;
                break;
            }
        }
        return boughtItem;
    }

    private ItemController CreateNewItem(ItemModel _model, int _count)
    {
        ItemModel itemModel = new ItemModel(_model.id, _model.type, _model.rarity,
                                                _model.icon, _model.description, _model.buyingPrice,
                                                _model.sellingPrice, _model.weight, _count);

        ItemController itemController = new ItemController(itemModel, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);

        inventoryModel.itemsList.Add(itemController);

        itemController.GetItemView().itemButton.onClick.AddListener(()=>OnItemButtonClicked(itemModel));

        return itemController;
    }
    public void CreateNewItem(ItemModel _model)
    {
        ItemController itemController = new ItemController(_model, inventoryView.itemButtonPrefab, inventoryView.itemButtonsParent.transform);
        inventoryModel.itemsList.Add(itemController);
        itemController.GetItemView().itemButton.onClick.AddListener(() => OnItemButtonClicked(_model));
    }

    private void DeleteItem(ItemController toBeDeleted)
    {
        GetInventoryModel().itemsList.Remove(toBeDeleted);
        toBeDeleted.DestroyItem();
    }
}