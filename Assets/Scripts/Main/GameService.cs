using System.Collections.Generic;
using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    //Services
    public EventService EventService { get; private set; }
    public ShopService ShopService { get; private set; }
    public InventoryService InventoryService { get; private set; }
    public PopupService PopupService { get; private set; }

    private GatherResourcesController gatherResourcesController;//one time use button
    
    [Header("Item Scriptable Objects")]
    [SerializeField]private List<ItemScriptableObject> itemsSOList;

    [Header("Shop and Inventory Prefabs")]
    [SerializeField] private ShopView shopPrefab;
    [SerializeField] private InventoryView inventoryPrefab;
    [SerializeField] private GatherResourcesController gatherResourcesPrefab;
    [SerializeField] private ItemDetailsController itemDetailsPrefab;
    [SerializeField] private ItemCountSetterController itemCountSetterPrefab;
    [SerializeField] private ConfirmationRequestController confirmationRequestPrefab;
    [SerializeField] private TransactionCompleteController transactionCompletePrefab;

    [Header("Scene References")]
    [SerializeField] private Transform shopAndInventoryParent;

    private void Start() => CreateServices();

    private void CreateServices()
    {
        EventService = new EventService();

        ShopService = new ShopService(itemsSOList, shopPrefab, shopAndInventoryParent);

        InventoryService = new InventoryService(inventoryPrefab, shopAndInventoryParent);

        PopupService = new PopupService(itemDetailsPrefab, itemCountSetterPrefab, 
            confirmationRequestPrefab, transactionCompletePrefab, shopAndInventoryParent);

        gatherResourcesController = GameObject.Instantiate(gatherResourcesPrefab,shopAndInventoryParent);
        gatherResourcesController.Initialize(itemsSOList);
    }
}