using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/NewItemScriptableObject")]
public class ItemScriptableObject:ScriptableObject
{
    public ItemTypes itemType;
    public ItemRarity itemRarity;

    public Sprite icon;

    public string description;
    public int buyingPrice;
    public int sellingPrice;
    public int weight;
    public int quantity;
}