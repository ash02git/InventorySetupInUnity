using System.Collections.Generic;
using UnityEngine;
public class UIService : MonoBehaviour
{
    //Dependencies
    private ShopController shopController;
    private InventoryController inventoryController;
    private ItemDetailsController itemDetailsController;
    private ItemCountSetterController itemCountSetterController;
    private ConfirmationRequestController confirmationRequestController;
    private TransactionCompleteController transactionCompleteController;

    //Item SO list
    public List<ItemScriptableObject> itemsSO;

    //Prefabs
    public ShopView shopViewPrefab;
    public InventoryView inventoryViewPrefab;

    public ItemDetailsController itemDetailsPanelPrefab;
    public ItemCountSetterController itemCountSetterPanelPrefab;
    public ConfirmationRequestController confirmationRequestPanelPrefab;
    public TransactionCompleteController transactionCompleteOverlayPrefab;

    private void Start()
    {
        CreateControllers();
        InjectDependencies();
    }
    private void CreateControllers()
    {
        //creating shopController
        ShopModel shopModel = new ShopModel();
        shopController = new ShopController(itemsSO, shopModel, shopViewPrefab, transform);

        //creating inventoryController
        InventoryModel inventoryModel = new InventoryModel();
        inventoryController = new InventoryController(inventoryModel, inventoryViewPrefab, transform);

        //creating itemDetailsController
        itemDetailsController = GameObject.Instantiate<ItemDetailsController>(itemDetailsPanelPrefab, transform);

        //creating itemCountSetterController
        itemCountSetterController = GameObject.Instantiate<ItemCountSetterController>(itemCountSetterPanelPrefab, transform);

        //creating confirmationRequestController
        confirmationRequestController = GameObject.Instantiate<ConfirmationRequestController>(confirmationRequestPanelPrefab, transform);

        //creating transactionCompleteController
        transactionCompleteController = GameObject.Instantiate<TransactionCompleteController>(transactionCompleteOverlayPrefab, transform);
    }
    private void InjectDependencies()
    {
        //init for shopController
        shopController.Init(itemDetailsController);

        //init for inventoryController
        inventoryController.Init(itemDetailsController);

        //init for itemDetailsController
        itemDetailsController.Init(itemCountSetterController);

        //init for itemCountSetterController
        itemCountSetterController.Init(confirmationRequestController, inventoryController);

        //init for confirmationRequestController
        confirmationRequestController.Init(shopController, inventoryController, itemDetailsController,
            itemCountSetterController, transactionCompleteController);
    }

    public void OnItemButtonClicked(ItemModel _model, ItemContext _context)
    {
        itemDetailsController.gameObject.SetActive(true);
        itemDetailsController.UpdateDetails(_model, _context);
    }
    public void OnActionButtonClicked(ItemModel _model, ItemContext _context)
    {
        itemCountSetterController.gameObject.SetActive(true);
        itemCountSetterController.UpdateDetails(_model, _context);
    }
    public void OnCountValueSet(ItemModel _model, ItemContext _context, int _count)
    {
        confirmationRequestController.gameObject.SetActive(true);
        confirmationRequestController.UpdateDetails(_model, _context, _count);
    }
}
