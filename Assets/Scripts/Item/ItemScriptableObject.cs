using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/NewItemScriptableObject")]
public class ItemScriptableObject:ScriptableObject
{
    public ItemID id;
    public ItemType type;
    public ItemRarity rarity;
    public DoableAction doableAction;
    public Sprite icon;
    public string description;
    public int buyingPrice;
    public int sellingPrice;
    public int weight;
    public int quantity;
}