using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsController : MonoBehaviour
{
    //dependency
    private ItemCountSetterController itemCountSetterController;

    //data required for ItemDetailsController
    private ItemContext itemContext;
    //private ItemModel itemModel;//this is being replaced by ItemScriptableObject itemDisplayDetails
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

    public void Init(ItemCountSetterController itemCountSetterController)
    {
        this.itemCountSetterController = itemCountSetterController;
    }

    private void OnEnable()
    {
        actionButton.onClick.AddListener(() => OnActionButtonClicked(itemDetails, itemContext));//previously first paramter was ItemModel
    }
    private void OnDisable()
    {
        actionButton.onClick.RemoveAllListeners();
    }

    public void UpdateDetails(ItemScriptableObject itemSO, ItemContext _context)//previously first paramter was ItemModel
    {
        itemContext = _context;
        //itemModel = _model;
        this.itemDetails = itemSO;

        itemNameText.text = "Item Name : " + itemDetails.id.ToString();
        itemDescriptionText.text = "Item Description : " + itemDetails.description;
        itemIconImage.sprite = itemDetails.icon;
        itemTypeText.text = "Item Type : " + itemDetails.type.ToString();
        itemRarityText.text = "Item Rarity : " + itemDetails.rarity.ToString();
        Debug.Log(itemDetails.rarity);
        itemWeightText.text = "Item Weight : " + itemDetails.weight.ToString();
        itemQuantityText.text = "Item Quantity : " + itemDetails.quantity.ToString();

        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = _context.ToString();

        switch (_context)
        {
            case ItemContext.Buy:
                itemPriceText.text = "Item Buying Price : " + itemDetails.buyingPrice;
                break;
            case ItemContext.Sell:
                itemPriceText.text = "Item Selling Price : " + itemDetails.sellingPrice;
                break;
        }
    }
    private void OnActionButtonClicked(ItemScriptableObject itemSO, ItemContext _context)//previously first parameter was ItemModel
    {
        itemCountSetterController.gameObject.SetActive(true);
        itemCountSetterController.UpdateDetails(itemSO, _context);//previously first paramter was ItemModel
    }
}
