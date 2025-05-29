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
    private GatherResourcesController gatherResourcesController;

    //Item SO list
    public List<ItemScriptableObject> itemsSO;

    //Prefabs
    public ShopView shopViewPrefab;
    public InventoryView inventoryViewPrefab;

    public ItemDetailsController itemDetailsPanelPrefab;
    public ItemCountSetterController itemCountSetterPanelPrefab;
    public ConfirmationRequestController confirmationRequestPanelPrefab;
    public TransactionCompleteController transactionCompleteOverlayPrefab;
    public GatherResourcesController gatherResourcesButtonPrefab;

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

        //creating gatherResourcesController
        gatherResourcesController = GameObject.Instantiate<GatherResourcesController>(gatherResourcesButtonPrefab, transform);
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

        //init for gatherResourcesController
        gatherResourcesController.Init(inventoryController, shopController, itemsSO);
    }
}
