using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ConfirmationRequestController : MonoBehaviour
{
    //dependency
    private ShopController shopController;
    private InventoryController inventoryController;
    private ItemDetailsController itemDetailsController;//to be turned off in the end
    private ItemCountSetterController itemCountSetterController;//to be turned off in the end
    private TransactionCompleteController transactionCompleteController;//to be turned on in the end

    //Data required for ConfirmationRequestController
    private int itemCount;
    private ItemModel itemModel;
    private ItemContext itemContext;

    //GameObject references
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button yesButton;
    private void OnEnable()
    {
        yesButton.onClick.AddListener(() => OnYesButtonClicked(itemModel, itemContext, itemCount));
    }
    private void OnDisable()
    {
        yesButton.onClick.RemoveAllListeners();
    }
    public void Init(ShopController shopController, InventoryController inventoryController,
        ItemDetailsController itemDetailsController, ItemCountSetterController itemCountSetterController,
        TransactionCompleteController transactionCompleteController)
    {
        this.shopController = shopController;
        this.inventoryController = inventoryController;
        this.itemDetailsController = itemDetailsController;
        this.itemCountSetterController = itemCountSetterController;
        this.transactionCompleteController = transactionCompleteController;
    }

    public void UpdateDetails(ItemModel model, ItemContext context, int count)
    {
        itemCount = count;
        itemModel = model;
        itemContext = context;

        itemIconImage.sprite = itemModel.icon;
        confirmationText.text = "Are you sure you want to " + itemContext.ToString() + " " + itemCount + " " + itemModel.id.ToString() + "(s) for ";

        if (itemContext == ItemContext.Buy)
        {
            confirmationText.text += (itemCount * itemModel.buyingPrice) + " gold?";
        }
        else
        {
            confirmationText.text += (itemCount * itemModel.sellingPrice) + " gold?";
        }
    }

    private void OnYesButtonClicked(ItemModel passedModel, ItemContext passedContext, int passedCount)
    {
        SwitchOffPreviousPopUps();
        PassDetailsToShopAndInventory(passedModel,passedContext,passedCount);
        DisplayOverlayMessage(passedModel.id, passedContext);
    }
    private void SwitchOffPreviousPopUps()
    {
        itemDetailsController.gameObject.SetActive(false);
        itemCountSetterController.gameObject.SetActive(false);
    }
    private void PassDetailsToShopAndInventory(ItemModel _model, ItemContext _context, int _count)
    {
        inventoryController.OnTransactionPerformed(_model, _context, _count);
        shopController.OnTransactionPerformed(_model, _context, _count);
    }
    private void DisplayOverlayMessage(ItemID _id, ItemContext _context)
    {
        transactionCompleteController.gameObject.SetActive(true);
        transactionCompleteController.DisplayOverlay(_id, _context);
    }
}
