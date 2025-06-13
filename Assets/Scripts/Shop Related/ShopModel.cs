using System;
using System.Collections.Generic;

public class ShopModel
{
    private List<ItemController> itemsList;

    public ShopModel()
    {
        itemsList = new List<ItemController>();
    }

    public List<ItemController> GetItemsList() => itemsList;

    public void AddItem(ItemController toBeAdded) => itemsList.Add(toBeAdded);
}