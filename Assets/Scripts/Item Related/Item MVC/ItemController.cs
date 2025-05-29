using UnityEngine;

public class ItemController
{
    //references to model and view
    private ItemModel itemModel;
    private ItemView itemView;

    public ItemController(ItemModel _model, ItemView _view, Transform parent)//constructor - ItemModel, ItemView Prefab and Parent Transform as parameters
    {
        itemModel = _model;
        itemView = GameObject.Instantiate<ItemView>(_view, parent);

        itemModel.SetItemController(this);
        itemView.SetItemController(this);
    }
    public ItemView GetItemView()
    {
        return itemView;
    }
    public ItemModel GetItemModel()
    {
        return itemModel;
    }

    public void DisplayChangedQuantityText()
    {
        itemView.UpdateQuantityText();
    }
    public void DestroyItem()
    {
        GameObject.Destroy(itemView.gameObject);
    }
}