using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    //reference to Item Controller
    private ItemController itemController;

    //reference to components
    [SerializeField] private Button itemButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    public void SetItemController(ItemController itemController)=> this.itemController = itemController;

    private void Start()
    {
        iconImage.sprite = itemController.GetIcon();
        quantityText.text = "x" + itemController.GetQuantity();
    }

    public Button GetItemButton() => itemButton;

    public void UpdateQuantityText() => quantityText.text = "x " + itemController.GetQuantity();

    public void SetItemButtonNonInteractable() => itemButton.interactable = false;

    public void SetItemButtonInteractable() => itemButton.interactable = true;

    private void OnDestroy() => itemButton.onClick.RemoveAllListeners();
}
