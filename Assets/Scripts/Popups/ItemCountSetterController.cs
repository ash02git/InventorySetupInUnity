using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCountSetterController : MonoBehaviour
{
    //dependency
    //private ConfirmationRequestController confirmationRequestController;
    //private InventoryController inventoryController;

    //Data required for ItemCountSetterController
    //private ItemContext itemContext;
    private int itemCount;
    private ItemScriptableObject itemSO;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button confirmButton;//most important Component in ItemCountSetterController
    [SerializeField] private TextMeshProUGUI errorText;

    //public void Init(ConfirmationRequestController confirmationRequestController, InventoryController inventoryController)
    //{
    //    this.confirmationRequestController = confirmationRequestController;
    //    this.inventoryController = inventoryController;
    //}

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(() => OnCountValueSet(itemSO, itemCount));
        minusButton.onClick.AddListener(OnMinusButtonClicked);
        plusButton.onClick.AddListener(OnPlusButtonClicked);

        //EventService.Instance.OnTransactionCompleted.AddListener(OnTransactionCompleted);//maybe make this as a function
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveListener(() => OnCountValueSet(itemSO, itemCount));
        minusButton.onClick.RemoveListener(OnMinusButtonClicked);
        plusButton.onClick.RemoveListener(OnPlusButtonClicked);

        //EventService.Instance.OnTransactionCompleted.RemoveListener(OnTransactionCompleted);
    }

    public void UpdateDetails(ItemScriptableObject itemSO)
    {
        this.itemSO = itemSO;
        itemCount = 0;
        itemNameText.text = itemSO.id.ToString();
        itemIconImage.sprite = itemSO.icon;

        if (itemSO.doableAction == DoableAction.Buy)
        {
            itemPriceText.text = "Buying Price : 0";
            itemWeightText.text = "Weight Required : 0";
        }
        else
        {
            itemPriceText.text = "Selling Price : 0";
            itemWeightText.text = "Weight Released : 0";
        }

        itemQuantityText.text = itemCount.ToString();//changing itemQuantity text
        Check();//does the checks to determine whether items can be added or subtracted
    }

    public void OnPlusButtonClicked()
    {
        itemCount++;
        UpdateTextChanges();
        Check();//does the checks to determine whether items can be added or subtracted
    }

    public void OnMinusButtonClicked()
    {
        itemCount--;
        UpdateTextChanges();
        Check();//does the checks to determine whether items can be added or subtracted
    }

    private void UpdateTextChanges()
    {
        itemQuantityText.text = itemCount.ToString();//changing itemQuantity text

        if (itemSO.doableAction == DoableAction.Buy)
        {
            itemPriceText.text = "Buying Price : " + (itemSO.buyingPrice * itemCount);//changing itemPrice text
            itemWeightText.text = "Weight Required : " + (itemSO.weight * itemCount);//changing itemWeight text
        }
        else if (itemSO.doableAction == DoableAction.Sell)
        {
            itemPriceText.text = "Selling Price : " + (itemSO.sellingPrice * itemCount);//changing itemPrice text
            itemWeightText.text = "Weight Released : " + (itemSO.weight * itemCount);//changing itemWeight text
        }
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
        //Checks to limit addition of items based on Buy or Sell context
        if (itemSO.doableAction == DoableAction.Buy)
        {
            if (((itemCount + 1) * itemSO.buyingPrice > GameService.Instance.InventoryService.GetCurrency() || //check if enough currency is there
                ((itemCount + 1) * itemSO.weight + GameService.Instance.InventoryService.GetCurrentWeight() > GameService.Instance.InventoryService.GetMaxWeight() ) || //check if max weight is not breached
                ((itemCount + 1) > itemSO.quantity)) ) //check if enough items are there
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
        else if (itemSO.doableAction == DoableAction.Sell)
        {
            if (itemCount + 1 > itemSO.quantity)
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

    private void OnCountValueSet(ItemScriptableObject itemSO, int _count) => GameService.Instance.EventService.OnItemCountSet.InvokeEvent(itemSO, _count);
}