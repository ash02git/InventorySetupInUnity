using System;
using System.Collections.Generic;
public class InventoryModel
{
    //list of all items in Inventory
    public List<ItemController> itemsList;

    //Data specific to inventory
    public int currency;
    public int maxWeight;
    public int currentWeight;

    //reference to InventoryController
    private InventoryController inventoryController;

    public InventoryModel()
    {
        itemsList = new List<ItemController>();
        currency = 1000;//to be changed to 0 when GatherResourcesButton is implemented
        maxWeight = 180;
        currentWeight = 0;
    }
    public void SetInventoryController(InventoryController _controller)
    {
        inventoryController = _controller;
    }

    public void UpdateCurrency(int value)
    {
        currency += value;
    }

    public void UpdateWeight(int value)
    {
        currentWeight += value;
    }
}