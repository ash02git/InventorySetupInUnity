using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ShopAndInventory.Main;
using ShopAndInventory.Item;

namespace ShopAndInventory.Popup
{
    public class ItemDetailsController : MonoBehaviour
    {
        //data required for ItemDetailsController
        private ItemScriptableObject itemDetails;

        //reference to components
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Image itemIconImage;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI itemRarityText;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI itemQuantityText;
        [SerializeField] private TextMeshProUGUI itemWeightText;
        [SerializeField] private Button actionButton;

        private void OnEnable() => actionButton.onClick.AddListener(() => OnActionButtonClicked(itemDetails));

        private void OnDisable() => actionButton.onClick.RemoveAllListeners();

        public void UpdateDetails(ItemScriptableObject itemSO)
        {
            this.itemDetails = itemSO;

            itemNameText.text = "Item Name : " + itemDetails.id.ToString();
            itemDescriptionText.text = "Item Description : " + itemDetails.description;
            itemIconImage.sprite = itemDetails.icon;
            itemTypeText.text = "Item Type : " + itemDetails.type.ToString();
            itemRarityText.text = "Item Rarity : " + itemDetails.rarity.ToString();
            itemWeightText.text = "Item Weight : " + itemDetails.weight.ToString();
            itemQuantityText.text = "Item Quantity : " + itemDetails.quantity.ToString();

            switch (itemSO.doableAction)
            {
                case DoableAction.Buy:
                    actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
                    itemPriceText.text = "Item Buying Price : " + itemDetails.buyingPrice;
                    break;
                case DoableAction.Sell:
                    actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell";
                    itemPriceText.text = "Item Selling Price : " + itemDetails.sellingPrice;
                    break;
            }
        }

        private void OnActionButtonClicked(ItemScriptableObject itemSO) => GameService.Instance.EventService.OnItemActionInitiated.InvokeEvent(itemSO);
    }
}