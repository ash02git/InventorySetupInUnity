using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherResourcesController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject failedText;

    private List<ItemModel> itemsModelList;

    private readonly int[] cumlativeValueNeededForDifferentItemRarities = { 0, 100, 150, 200, 300 };
    private readonly int[] probabilityOfDifferentItemTypes = { 100, 90, 50, 25, 10 };//verycommon-100%,common-90%,rare-50%,epic-25%,legendary-10%

    private InventoryController inventoryController;
    private ShopController shopController;

    private List<ItemScriptableObject> itemsSO;

    public void Init(InventoryController inventoryController, ShopController shop_controller, List<ItemScriptableObject> itemsSO)
    {
        this.inventoryController = inventoryController;
        this.shopController = shop_controller;
        this.itemsSO = itemsSO;

        itemsModelList = new List<ItemModel>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnGatherResources);//Subscribing
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnGatherResources);//Unsubscribing
    }

    private void OnGatherResources()
    {
        int cumulativeWeightAfterGathering = GatherResourcesAsModels();

        if(!IsGatheredItemsOverWeight(cumulativeWeightAfterGathering))
        {
            CreateItemsInInventory(cumulativeWeightAfterGathering);
            inventoryController.AssignRandomCurrencyValue();
            shopController.TurnOnAllButtons();//making the buttons in shop interactable
            DestroyGatherResourcesButton();
        }
        else
        {
            ClearItemsList();
            StartCoroutine(GatheringResourcesFailedCoroutine());
        }
    }

    private int GatherResourcesAsModels()
    {
        int cumulativeValue = 0;
        int cumulativeWeight = 0;

        for (int i = 1; i <= 5; i++)//5 ItemRarity Types
        {
            if (IsItemRarityGatherable(cumulativeValue, i))//Checking if an item of rarity value i is attainable according to CumulativeValue math
            {
                foreach (ItemScriptableObject item in itemsSO)
                {
                    if (item.rarity == (ItemRarity)i)//Selectively checking for particular Rarity
                    {
                        if (IsProbabilitySatisfied(i))//Checking if selected item's probability is met
                        {
                            int numberOfItems = UnityEngine.Random.Range(0, item.quantity);

                            if (numberOfItems != 0)
                            {
                                CreateItemModel(item, numberOfItems);

                                cumulativeValue += numberOfItems * i;//here i is same as value assigned to each rarity type
                                cumulativeWeight += numberOfItems * item.weight;
                            }
                        }
                    }
                }
            }
            else
                break;
        }
        return cumulativeWeight;
    }

    private void CreateItemsInInventory(int cumulativeWeight)
    {
        foreach (ItemModel itemModel in itemsModelList)
        {
            inventoryController.CreateNewItem(itemModel);
        }
        inventoryController.GetInventoryModel().UpdateWeight(cumulativeWeight);
        inventoryController.GetInventoryView().UpdateWeightText();
    }

    IEnumerator GatheringResourcesFailedCoroutine()
    {
        failedText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        failedText.SetActive(false);
    }

    private bool IsItemRarityGatherable(int value, int index) => (value >= cumlativeValueNeededForDifferentItemRarities[index - 1]);
    
    private bool IsGatheredItemsOverWeight(int weight) => weight > inventoryController.GetInventoryModel().maxWeight;
    
    private bool IsProbabilitySatisfied(int index) => ( UnityEngine.Random.Range(1, 101) > (100 - probabilityOfDifferentItemTypes[index - 1]) );
    
    private void CreateItemModel(ItemScriptableObject item, int numberOfItems)
    {
        ItemModel newModel = new ItemModel(item.id, item.type, item.rarity,
                                                               item.icon, item.description, item.buyingPrice,
                                                               item.sellingPrice, item.weight, numberOfItems);

        itemsModelList.Add(newModel);
    }
    private void ClearItemsList()=>itemsModelList.Clear();
    private void DestroyGatherResourcesButton() => Destroy(gameObject);
}