using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    //reference to Item Controller
    private ItemController itemController;

    //reference to components
    public Button itemButton;
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    public void SetItemController(ItemController itemController)=> this.itemController = itemController;

    public ItemModel GetItemModel()=> itemController.GetItemModel();

    private void Start()
    {
        ItemModel _model = GetItemModel();
        iconImage.sprite = _model.icon;
        quantityText.text = "x " + _model.quantity;
    }

    public void UpdateQuantityText() => quantityText.text = "x " + GetItemModel().quantity;

    private void OnDestroy() => itemButton.onClick.RemoveAllListeners();
}
