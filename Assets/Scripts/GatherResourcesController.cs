using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherResourcesController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject failedText;

    private List<ItemScriptableObject> itemsSOList;

    private readonly int[] ItemRaritiesCumulativeValues = { 0, 100, 150, 200, 300 };
    private readonly int[] ItemTypesProbabilities = { 100, 90, 50, 25, 10 };//verycommon-100%,common-90%,rare-50%,epic-25%,legendary-10%

    private InventoryController inventoryController;
    private ShopController shopController;

    private List<ItemScriptableObject> itemsSO;

    public void Init(InventoryController inventoryController, ShopController shopController, List<ItemScriptableObject> itemsSO)
    {
        this.inventoryController = inventoryController;
        this.shopController = shopController;
        this.itemsSO = itemsSO;

        //itemsModelList = new List<ItemModel>();
        itemsSOList = new List<ItemScriptableObject>();
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
        int cumulativeWeightAfterGathering = GatherResourcesAsItemSOs();

        if(!IsGatheredItemsOverWeight(cumulativeWeightAfterGathering))
        {
            CreateItemsInInventory(cumulativeWeightAfterGathering);
            inventoryController.AssignRandomCurrencyValue();
            shopController.MakeItemButtonsInteractable();//making the buttons in shop interactable
            DestroyGatherResourcesButton();
        }
        else
        {
            ClearItemsList();
            StartCoroutine(GatheringResourcesFailedCoroutine());
        }
    }

    private int GatherResourcesAsItemSOs()
    {
        int cumulativeValue = 0;
        int cumulativeWeight = 0;

        for (int i = 1; i <= 5; i++)//5 ItemRarity Types//replace with enum.getnumberofitemsinenum thing
        {
            if (IsItemRarityGatherable(cumulativeValue, i))//Checking if an item of rarity value i is attainable according to CumulativeValue math
            {
                //Debug.Log((ItemRarity)i + " rarity is gatherable. Cumulative value is " + cumulativeValue);
                foreach (ItemScriptableObject item in itemsSO)
                {
                    if (item.rarity == (ItemRarity)i)//Selectively checking for particular Rarity
                    {
                        if (IsProbabilitySatisfied(i))//Checking if selected item's probability is met
                        {
                            //Debug.Log("Number of items as preset is " + item.quantity);
                            int numberOfItems = Random.Range(0, item.quantity);
                            //Debug.Log("Random item count generated is "+ numberOfItems);//comment to check working

                            if (numberOfItems != 0)
                            {
                                CreateItemSO(item, numberOfItems);

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
        foreach (ItemScriptableObject itemSO in itemsSOList)
        {
            inventoryController.CreateNewItem(itemSO);
        }

        inventoryController.UpdateWeight(cumulativeWeight);
        inventoryController.UpdateWeightText();
    }

    IEnumerator GatheringResourcesFailedCoroutine()
    {
        failedText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        failedText.SetActive(false);
    }

    private bool IsItemRarityGatherable(int value, int index) => (value >= ItemRaritiesCumulativeValues[index - 1]);
    
    private bool IsGatheredItemsOverWeight(int weight) => weight > inventoryController.GetMaxWeight();
    
    private bool IsProbabilitySatisfied(int index) => ( UnityEngine.Random.Range(1, 101) > (100 - ItemTypesProbabilities[index - 1]) );
    
    private void CreateItemSO(ItemScriptableObject item, int numberOfItems)
    {
        //ItemModel newModel = new ItemModel(item.id, item.type, item.rarity,
        //                                                       item.icon, item.description, item.buyingPrice,
        //                                                       item.sellingPrice, item.weight, numberOfItems);
        ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(item), newItemSO);

        newItemSO.quantity = numberOfItems;//important line

        //itemsModelList.Add(newModel);//previously first parameter was ItemModel
        itemsSOList.Add(newItemSO);
    }

    private void ClearItemsList()=>itemsSOList.Clear();

    private void DestroyGatherResourcesButton() => Destroy(gameObject);
}