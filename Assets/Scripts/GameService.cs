using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    //Services
    private SoundService soundService;
    private EventService eventService;
    private ShopAndInventoryService shopAndInventoryService;
    
    [Header("AudioSource")]
    [SerializeField] private AudioSource SfxSource;
    
    [Header("Item Scriptable Objects")]
    [SerializeField]private List<ItemScriptableObject> itemsSOList;

    [Header("ScriptableObjects")]
    [SerializeField] private SoundScriptableObject soundScriptableObject;

    [Header("Prefabs")]
    [SerializeField] private ShopView shopPrefab;
    [SerializeField] private InventoryView inventoryPrefab;
    [SerializeField] private GatherResourcesController gatherResourcesPrefab;
    [SerializeField] private ItemDetailsController itemDetailsPrefab;
    [SerializeField] private ItemCountSetterController itemCountSetterPrefab;
    [SerializeField] private ConfirmationRequestController confirmationRequestPrefab;

    [Header("Scene References")]
    [SerializeField] private Transform shopAndInventoryParent;

    private void CreateServices()
    {
        eventService = new EventService();
        soundService = new SoundService(soundScriptableObject, SfxSource);
        shopAndInventoryService = new ShopAndInventoryService(itemsSOList, shopPrefab, inventoryPrefab, shopAndInventoryParent);
    }

    private void Start() => CreateServices();
}