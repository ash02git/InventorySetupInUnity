using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/NewItemScriptableObject")]
public class ItemScriptableObject:ScriptableObject
{
    public ItemID id;//unqiue value for each item
    
    public ItemType type;
    public ItemRarity rarity;

    public Sprite icon;

    public string i_name;
    public string description;
    public int buyingPrice;
    public int sellingPrice;
    public int weight;
    public int quantity;
}