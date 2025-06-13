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
    //private ItemModel itemModel;//this will be replaced by ItemScriptableObject
    private ItemScriptableObject itemDetails;

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
        confirmButton.onClick.AddListener(() => OnCountValueSet(itemDetails, itemContext, itemCount));//previously first parameter was ItemModel
    }
    private void OnDisable()
    {
        confirmButton.onClick.RemoveAllListeners();
    }
    public void UpdateDetails(ItemScriptableObject itemSO, ItemContext _context)//previously first parameter was ItemModel
    {
        itemDetails = itemSO;
        itemContext = _context;
        itemCount = 0;
        itemNameText.text = itemSO.id.ToString();
        itemIconImage.sprite = itemSO.icon;

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
            itemPriceText.text = "Buying Price : "+ (itemDetails.buyingPrice * itemCount);
        else
            itemPriceText.text = "Selling Price : " + (itemDetails.sellingPrice * itemCount);
    }
    private void UpdateWeightText()
    {
        if (itemContext == ItemContext.Buy)
            itemWeightText.text = "Weight Required : " + (itemDetails.weight * itemCount);
        else
            itemWeightText.text = "Weight Released : " + (itemDetails.weight * itemCount);
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
            if (((itemCount + 1) * itemDetails.buyingPrice > i_model.currency) ||
                ((itemCount + 1) * itemDetails.weight + i_model.currentWeight > i_model.maxWeight) ||
                ((itemCount + 1) > itemDetails.quantity))
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
            if (itemCount + 1 > itemDetails.quantity)
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
    private void OnCountValueSet(ItemScriptableObject itemSO, ItemContext _context, int _count)//previously first parameter was ItemModel
    {
        confirmationRequestController.gameObject.SetActive(true);
        confirmationRequestController.UpdateDetails(itemSO, _context, _count);//previously first parameter was ItemModel
    }
}
