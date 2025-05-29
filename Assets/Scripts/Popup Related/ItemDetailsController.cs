using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsController : MonoBehaviour
{
    //dependency
    private ItemCountSetterController itemCountSetterController;

    //data required for ItemDetailsController
    private ItemContext itemContext;
    private ItemModel itemModel;

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
        actionButton.onClick.AddListener(() => OnActionButtonClicked(itemModel, itemContext));
    }
    private void OnDisable()
    {
        actionButton.onClick.RemoveAllListeners();
    }

    public void UpdateDetails(ItemModel _model, ItemContext _context)
    {
        itemContext = _context;
        itemModel = _model;

        itemNameText.text = "Item Name : " + _model.id.ToString();
        itemDescriptionText.text = "Item Description : " + _model.description;
        itemIconImage.sprite = _model.icon;
        itemTypeText.text = "Item Type : " + _model.type.ToString();
        itemRarityText.text = "Item Rarity : " + _model.rarity.ToString();
        Debug.Log(_model.rarity);
        itemWeightText.text = "Item Weight : " + _model.weight.ToString();
        itemQuantityText.text = "Item Quantity : " + _model.quantity.ToString();

        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = _context.ToString();

        switch (_context)
        {
            case ItemContext.Buy:
                itemPriceText.text = "Item Buying Price : " + _model.buyingPrice;
                break;
            case ItemContext.Sell:
                itemPriceText.text = "Item Selling Price : " + _model.sellingPrice;
                break;
        }
    }
    private void OnActionButtonClicked(ItemModel _model, ItemContext _context)
    {
        itemCountSetterController.gameObject.SetActive(true);
        itemCountSetterController.UpdateDetails(_model, _context);
    }
}
