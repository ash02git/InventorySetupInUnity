using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountSetterController : MonoBehaviour
{
    //dependency
    private ConfirmationRequestController confirmationRequestController;
    private InventoryController inventoryController;

    //Data required for ItemCountSetterController
    private ItemContext itemContext;
    private int itemCount;
    private ItemModel itemModel;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI errorText;

    public void Init(ConfirmationRequestController confirmationRequestController, InventoryController inventoryController)
    {
        this.confirmationRequestController = confirmationRequestController;
        this.inventoryController = inventoryController;
    }
    private void OnEnable()
    {
        confirmButton.onClick.AddListener(() => OnCountValueSet(itemModel, itemContext, itemCount));
    }
    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
    }
    public void UpdateDetails(ItemModel _model, ItemContext _context)
    {
        itemModel = _model;
        itemContext = _context;
        itemCount = 0;
        itemNameText.text = _model.id.ToString();
        itemIconImage.sprite = _model.icon;

        if (_context == ItemContext.Buy)
        {
            itemPriceText.text = "Buying Price : 0";
            itemWeightText.text = "Weight Required : 0";
        }
        else
        {
            itemPriceText.text = "Selling Price : 0";
            itemWeightText.text = "Weight Released : 0";
        }
        
        UpdateQuantityText();

        Check();
    }
    public void PlusQuantity()
    {
        itemCount++;
        UpdateTextChanges();
        Check();//does the checks to determine whether items can be added or subtracted
    }
    public void MinusQuantity()
    {
        itemCount--;
        UpdateTextChanges();
        Check();//does the checks to determine whether items can be added or subtracted
    }
    private void UpdateTextChanges()
    {
        UpdateQuantityText();
        UpdatePriceText();
        UpdateWeightText();
    }
    private void UpdateQuantityText()
    {
        itemQuantityText.text = itemCount.ToString();
    }
    private void UpdatePriceText()
    {
        if(itemContext == ItemContext.Buy)
            itemPriceText.text = "Buying Price : "+ (itemModel.buyingPrice * itemCount);
        else
            itemPriceText.text = "Selling Price : " + (itemModel.sellingPrice * itemCount);
    }
    private void UpdateWeightText()
    {
        if (itemContext == ItemContext.Buy)
            itemWeightText.text = "Weight Required : " + (itemModel.weight * itemCount);
        else
            itemWeightText.text = "Weight Released : " + (itemModel.weight * itemCount);
    }
    private void Check()
    {
        CheckForMinusButtonInteractability();
        CheckForPlusButtonInteractability();
    }
    private void CheckForMinusButtonInteractability()
    {
        if (itemCount == 0)
        {
            minusButton.interactable = false;
            confirmButton.interactable = false;
        }
        else
        {
            minusButton.interactable = true;
            confirmButton.interactable = true;
        }
    }
    private void CheckForPlusButtonInteractability()
    {
        InventoryModel i_model = inventoryController.GetInventoryModel();
        //Checks to limit addition of items based on Buy or Sell context
        if (itemContext == ItemContext.Buy)
        {
            if (((itemCount + 1) * itemModel.buyingPrice > i_model.currency) ||
                ((itemCount + 1) * itemModel.weight + i_model.currentWeight > i_model.maxWeight) ||
                ((itemCount + 1) > itemModel.quantity))
            {
                plusButton.interactable = false;
                errorText.gameObject.SetActive(true);
            }
            else
            {
                plusButton.interactable = true;
                errorText.gameObject.SetActive(false);
            }
        }
        else if (itemContext == ItemContext.Sell)
        {
            if (itemCount + 1 > itemModel.quantity)
            {
                plusButton.interactable = false;
                errorText.gameObject.SetActive(true);
            }
            else
            {
                plusButton.interactable = true;
                errorText.gameObject.SetActive(false);
            }
        }
    }
    private void OnCountValueSet(ItemModel _model, ItemContext _context, int _count)
    {
        confirmationRequestController.gameObject.SetActive(true);
        confirmationRequestController.UpdateDetails(_model, _context, _count);
    }
}
