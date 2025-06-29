using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ConfirmationRequestController : MonoBehaviour
{
    //Data required for ConfirmationRequestController
    private int itemCount;
    private ItemScriptableObject itemSO;

    //GameObject references
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button yesButton;
    private void OnEnable() => yesButton.onClick.AddListener(OnYesButtonClicked);

    private void OnDisable() => yesButton.onClick.RemoveListener(OnYesButtonClicked);

    public void UpdateDetails(ItemScriptableObject itemSO, int count)
    {
        itemCount = count;
        this.itemSO = itemSO;

        itemIconImage.sprite = this.itemSO.icon;
        confirmationText.text = "Are you sure you want to " + (itemSO.doableAction == DoableAction.Buy?"Buy":"Sell")  + " " + itemCount + " " + this.itemSO.id.ToString() + "(s) for ";

        if (itemSO.doableAction == DoableAction.Buy)
        {
            confirmationText.text += (itemCount * this.itemSO.buyingPrice) + " gold?";
        }
        else
        {
            confirmationText.text += (itemCount * this.itemSO.sellingPrice) + " gold?";
        }
    }

    private void OnYesButtonClicked()
    {
        if(itemSO.doableAction == DoableAction.Sell)
            GameService.Instance.EventService.OnItemSold.InvokeEvent(itemSO,itemCount);
        else if(itemSO.doableAction == DoableAction.Buy)
            GameService.Instance.EventService.OnItemBought.InvokeEvent(itemSO,itemCount);

        GameService.Instance.EventService.OnTransactionCompleted.InvokeEvent();
    }
}