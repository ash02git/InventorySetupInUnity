using UnityEngine;
using UnityEngine.UI;

public class ItemController
{
    //references to model and view
    private ItemScriptableObject itemSO;
    private ItemView itemView;

    public ItemController(ItemScriptableObject itemSO, ItemView itemView, Transform parent)
    {
        this.itemSO = itemSO;
        this.itemView = GameObject.Instantiate<ItemView>(itemView, parent);

        this.itemView.SetItemController(this);
    }

    public Sprite GetIcon() => itemSO.icon;

    public ItemType GetItemType() => itemSO.type;

    public int GetBuyingPrice() => itemSO.buyingPrice;

    public int GetSellingPrice() => itemSO.sellingPrice;

    public int GetWeight() => itemSO.weight;

    public ItemID GetID() => itemSO.id;

    public int GetQuantity() => itemSO.quantity;

    public Button GetItemButton() => itemView.GetItemButton();

    public bool HasNoItems() => itemSO.quantity <= 0;

    public void UpdateShopItemQuantity(int count,ItemContext context) //=> itemSO.quantity += count;
    {
        if(context == ItemContext.Buy)
        {
            itemSO.quantity -= count;
        }
        else if(context == ItemContext.Sell)
        {
            itemSO.quantity += count;
        }

        itemView.UpdateQuantityText();
    }

    public void UpdateInventoryItemQuantity(int count,ItemContext context) //=> itemSO.quantity -= count;
    {
        if (context == ItemContext.Buy)
        {
            itemSO.quantity += count;
        }
        else if (context == ItemContext.Sell)
        {
            itemSO.quantity -= count;
        }

        itemView.UpdateQuantityText();
    }

    public void SetItemButtonNonInteractable() => itemView.SetItemButtonNonInteractable();

    public void SetItemButtonInteractable() => itemView.SetItemButtonInteractable();

    public void HideItemButton() => itemView.gameObject.SetActive(false);

    public void ShowItemButton() => itemView.gameObject.SetActive(true);

    public void DisplayChangedQuantityText()
    {
        itemView.UpdateQuantityText();
    }

    public void DestroyItem()
    {
        GameObject.Destroy(itemView.gameObject);
    }
}