using UnityEngine;
public class ItemModel
{
    //reference to Item Controller
    private ItemController itemController;

    //Data
    public ItemID id;//unqiue value for each item

    public ItemType type;
    public ItemRarity rarity;

    public Sprite icon;

    public string description;
    public int buyingPrice;
    public int sellingPrice;
    public int weight;
    public int quantity;

    public ItemModel(ItemID id, ItemType type, ItemRarity rarity,
        Sprite icon, string description, int buyingPrice,
        int sellingPrice, int weight, int quantity)
    {
        this.id = id;
        this.type = type;
        this.icon = icon;
        this.description = description;
        this.buyingPrice = buyingPrice;
        this.sellingPrice = sellingPrice;
        this.weight = weight;
        this.quantity = quantity;
        this.rarity = rarity;
    }
    public void SetItemController(ItemController itemController)=>this.itemController = itemController;
}