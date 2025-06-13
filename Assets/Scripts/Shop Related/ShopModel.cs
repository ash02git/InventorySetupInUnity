using System;
using System.Collections.Generic;

public class ShopModel
{
    //list of all the items in Shop
    public List<ItemController> itemsList;

    //reference to ShopController
    private ShopController shopController;

    public ShopModel()
    {
        itemsList = new List<ItemController>();
    }

    public void SetShopController(ShopController _controller)
    {
        shopController = _controller;
    }
}