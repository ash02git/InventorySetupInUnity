using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsController : MonoBehaviour
{
    //dependency
    private ItemCountSetterController itemCountSetterController;

    //data required for ItemDetailsController
    private ItemContext itemContext;
    private ItemScriptableObject itemDetails;

    //reference to components
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemRarityText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private Button actionButton;

    public void Init(ItemCountSetterController itemCountSetterController) => this.itemCountSetterController = itemCountSetterController;

    private void OnEnable()
    {
        actionButton.onClick.AddListener(() => OnActionButtonClicked(itemDetails, itemContext));
        EventService.Instance.OnTransactionCompleted.AddListener(OnTransactionCompleted);//maybe make this as a function
    }

    private void OnDisable()
    {
        actionButton.onClick.RemoveAllListeners();
        EventService.Instance.OnTransactionCompleted.RemoveListener(OnTransactionCompleted);
    }

    public void UpdateDetails(ItemScriptableObject itemSO, ItemContext itemContext)
    {
        this.itemContext = itemContext;
        this.itemDetails = itemSO;

        itemNameText.text = "Item Name : " + itemDetails.id.ToString();
        itemDescriptionText.text = "Item Description : " + itemDetails.description;
        itemIconImage.sprite = itemDetails.icon;
        itemTypeText.text = "Item Type : " + itemDetails.type.ToString();
        itemRarityText.text = "Item Rarity : " + itemDetails.rarity.ToString();
        itemWeightText.text = "Item Weight : " + itemDetails.weight.ToString();
        itemQuantityText.text = "Item Quantity : " + itemDetails.quantity.ToString();

        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = itemContext.ToString();

        switch (itemContext)
        {
            case ItemContext.Buy:
                itemPriceText.text = "Item Buying Price : " + itemDetails.buyingPrice;
                break;
            case ItemContext.Sell:
                itemPriceText.text = "Item Selling Price : " + itemDetails.sellingPrice;
                break;
        }
    }

    private void OnActionButtonClicked(ItemScriptableObject itemSO, ItemContext _context)
    {
        itemCountSetterController.gameObject.SetActive(true);
        itemCountSetterController.UpdateDetails(itemSO, _context);
    }

    private void OnTransactionCompleted() => gameObject.SetActive(false);
}
