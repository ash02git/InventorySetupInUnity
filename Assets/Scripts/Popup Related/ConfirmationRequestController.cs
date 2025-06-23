using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ConfirmationRequestController : MonoBehaviour
{
    //dependency
    private ShopController shopController;
    private InventoryController inventoryController;
    //private ItemDetailsController itemDetailsController;//to be turned off in the end
    //private ItemCountSetterController itemCountSetterController;//to be turned off in the end
    private TransactionCompleteController transactionCompleteController;//to be turned on in the end

    //Data required for ConfirmationRequestController
    private int itemCount;
    private ItemScriptableObject itemDetails;
    private ItemContext itemContext;

    //GameObject references
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button yesButton;
    private void OnEnable()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);//removed params - itemDetails, itemContext, itemCount
        EventService.Instance.OnTransactionCompleted.AddListener(() => gameObject.SetActive(false));//maybe make this as a function
        //EventService.Instance.OnTransactionCompleted.AddListener(() => transactionCompleteController.gameObject.SetActive(true));
        EventService.Instance.OnTransactionCompleted.AddListener(DisplayOverlayMessage);
    }

    private void OnDisable()
    {
        yesButton.onClick.RemoveAllListeners();//maybe remove only the added listener 
    }

    public void Init(ShopController shopController, InventoryController inventoryController,
        ItemDetailsController itemDetailsController, ItemCountSetterController itemCountSetterController,
        TransactionCompleteController transactionCompleteController)
    {
        this.shopController = shopController;
        this.inventoryController = inventoryController;
        //this.itemDetailsController = itemDetailsController;
        //this.itemCountSetterController = itemCountSetterController;
        this.transactionCompleteController = transactionCompleteController;
    }

    public void UpdateDetails(ItemScriptableObject itemSO, ItemContext context, int count)//previously first parameter was ItemModel
    {
        itemCount = count;
        itemDetails = itemSO;
        itemContext = context;

        itemIconImage.sprite = itemDetails.icon;
        confirmationText.text = "Are you sure you want to " + itemContext.ToString() + " " + itemCount + " " + itemDetails.id.ToString() + "(s) for ";

        if (itemContext == ItemContext.Buy)
        {
            confirmationText.text += (itemCount * itemDetails.buyingPrice) + " gold?";
        }
        else
        {
            confirmationText.text += (itemCount * itemDetails.sellingPrice) + " gold?";
        }
    }

    private void OnYesButtonClicked()//removing parameters as they can be accessed as variables
    {
        //SwitchOffPreviousPopUps();

        //invoke OnTransactionPerformed events
        EventService.Instance.OnTransactionCompleted.InvokeEvent();

        PassDetailsToShopAndInventory(itemDetails, itemContext, itemCount);//(passedItemSO,passedContext,passedCount);
        DisplayOverlayMessage();//(passedItemSO.id, passedContext); //totally removing params - itemDetails.id,itemContext
    }

    private void SwitchOffPreviousPopUps()
    {
        //itemDetailsController.gameObject.SetActive(false);
        //itemCountSetterController.gameObject.SetActive(false);
    }

    private void PassDetailsToShopAndInventory(ItemScriptableObject itemSO, ItemContext _context, int _count)//previously first parameter was ItemModel
    {
        switch(_context)
        {
            case ItemContext.Buy:
                inventoryController.OnItemBought(itemSO, _count);
                shopController.OnItemBought(itemSO, _count);
                break;

            case ItemContext.Sell:
                inventoryController.OnItemSold(itemSO,_count);
                shopController.OnItemSold(itemSO, _count);
                break;  
        }
        
        //inventoryController.OnTransactionPerformed(itemSO, _context, _count);
        //shopController.OnTransactionPerformed(itemSO, _context, _count);
    }

    private void DisplayOverlayMessage()//removed params - ItemID _id, ItemContext _context
    {
        transactionCompleteController.gameObject.SetActive(true);
        transactionCompleteController.DisplayOverlay(itemDetails.id,itemContext); //removed params - _id, _context
    }
}
