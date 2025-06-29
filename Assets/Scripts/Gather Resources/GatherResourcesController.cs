using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherResourcesController : MonoBehaviour
{
    [SerializeField] private Button gatherResourcesButton;
    [SerializeField] private GameObject failedText;

    private List<ItemScriptableObject> gatheredItemsSOList;
    int maxWeight;

    private readonly int[] ItemRaritiesCumulativeValues = { 0, 100, 150, 200, 300 };//verycommon-0,common-100,rare-150,epic-200,legendary-300
    private readonly int[] ItemTypesProbabilities = { 100, 90, 50, 25, 10 };//verycommon-100%,common-90%,rare-50%,epic-25%,legendary-10%

    private List<ItemScriptableObject> originalItemsSOList;

    public void Initialize(List<ItemScriptableObject> itemsSO)
    {
        originalItemsSOList = itemsSO;
        gatheredItemsSOList = new List<ItemScriptableObject>();
        maxWeight = GameService.Instance.InventoryService.GetMaxWeight();
    }

    private void Start() => gatherResourcesButton.onClick.AddListener(OnGatherResources);//Subscribing

    private void OnDestroy() => gatherResourcesButton.onClick.RemoveListener(OnGatherResources);//Unsubscribing

    private void OnGatherResources()
    {
        int cumulativeWeightAfterGathering = GatherResourcesAsItemSOs();

        if(!IsGatheredItemsOverWeight(cumulativeWeightAfterGathering))
        {
            CreateItemsInInventory(cumulativeWeightAfterGathering);

            GameService.Instance.InventoryService.AssignRandomCurrencyValue();

            GameService.Instance.ShopService.MakeItemButtonsInteractable();

            DestroyGatherResourcesButton();
        }
        else
        {
            ClearItemsList();
            StartCoroutine(GatheringResourcesFailedCoroutine());
        }
    }

    private int GatherResourcesAsItemSOs() //gathering resource as per designed formula involving cumulative values
    {
        int cumulativeValue = 0;
        int cumulativeWeight = 0;

        for (int i = 1; i <= Enum.GetValues(typeof(ItemRarity)).Length; i++)
        {
            if (IsItemRarityGatherable(cumulativeValue, i))//Checking if an item of rarity value i is attainable according to CumulativeValue math
            {
                foreach (ItemScriptableObject item in originalItemsSOList)
                {
                    if (item.rarity == (ItemRarity)i)//Selectively checking for particular Rarity
                    {
                        if (IsProbabilitySatisfied(i))//Checking if selected item's probability is met
                        {
                            int numberOfItems = UnityEngine.Random.Range(0, item.quantity);

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
        foreach (ItemScriptableObject itemSO in gatheredItemsSOList)
            GameService.Instance.InventoryService.CreateNewItem(itemSO);

        GameService.Instance.InventoryService.UpdateWeight(cumulativeWeight);
    }

    IEnumerator GatheringResourcesFailedCoroutine()
    {
        failedText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        failedText.SetActive(false);
    }

    private bool IsItemRarityGatherable(int value, int index) => (value >= ItemRaritiesCumulativeValues[index - 1]);
    
    private bool IsGatheredItemsOverWeight(int weight) => weight > maxWeight;
    
    private bool IsProbabilitySatisfied(int index) => ( UnityEngine.Random.Range(1, 101) > (100 - ItemTypesProbabilities[index - 1]) );
    
    private void CreateItemSO(ItemScriptableObject item, int numberOfItems)
    {
        ItemScriptableObject newItemSO = ScriptableObject.CreateInstance<ItemScriptableObject>();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(item), newItemSO);

        newItemSO.quantity = numberOfItems;
        newItemSO.doableAction = DoableAction.Sell;

        gatheredItemsSOList.Add(newItemSO);
    }

    private void ClearItemsList()=>gatheredItemsSOList.Clear();

    private void DestroyGatherResourcesButton() => Destroy(gameObject);
}