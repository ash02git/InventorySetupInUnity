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

    public void SetInventoryController(InventoryController _controller) => inventoryController = _controller;

    private void Start()
    {
        currencyText.text = "Currency : " + inventoryController.GetCurrency();
        weightText.text = "Weight : 0/" + inventoryController.GetMaxWeight();
    }

    public void UpdateCurrencyText() => currencyText.text = "Currency : " + inventoryController.GetCurrency();

    public void UpdateWeightText() => weightText.text = "Weight : " + inventoryController.GetCurrentWeight() + "/" + inventoryController.GetMaxWeight();
}
