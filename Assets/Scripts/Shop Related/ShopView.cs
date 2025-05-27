using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    //reference to ShopController
    private ShopController shopController;

    //references to GameObjects
    [SerializeField] public Transform itemTypeTabsParent;
    [SerializeField] public Transform itemButtonsParent;

    //Prefabs
    [SerializeField] public Button itemTypeTabPrefab;
    [SerializeField] public ItemView itemButtonPrefab;

    public void SetShopController(ShopController _controller)
    {
        shopController = _controller;
    }
}
