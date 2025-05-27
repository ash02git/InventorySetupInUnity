using System;
using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    //reference to ShopController
    private InventoryController inventoryController;

    //references to GameObjects
    [SerializeField] public Transform itemButtonsParent;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI weightText;

    //Prefabs
    [SerializeField] public ItemView itemButtonPrefab;

    public void SetInventoryController(InventoryController _controller)
    {
        inventoryController = _controller;

        currencyText.text = "Currency : " + inventoryController.GetInventoryModel().currency;
        weightText.text = "Weight : 0/" + inventoryController.GetInventoryModel().maxWeight;
    }

    public void UpdateCurrencyText()
    {
        currencyText.text = "Currency : " + inventoryController.GetInventoryModel().currency;
    }

    public void UpdateWeightText()
    {
        weightText.text = "Weight : " + inventoryController.GetInventoryModel().currentWeight + "/" + inventoryController.GetInventoryModel().maxWeight;
    }
}
