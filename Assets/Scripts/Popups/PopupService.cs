using UnityEngine;
using ShopAndInventory.Main;
using ShopAndInventory.Item;

namespace ShopAndInventory.Popup
{
    public class PopupService
    {
        private ItemDetailsController itemDetailsController;
        private ItemCountSetterController itemCountSetterController;
        private ConfirmationRequestController confirmationRequestController;
        private TransactionCompleteController transactionCompleteController;

        public PopupService(ItemDetailsController itemDetailsPrefab, ItemCountSetterController itemCountSetterPrefab,
            ConfirmationRequestController confirmationRequestPrefab, TransactionCompleteController transactionCompletePrefab, Transform parent)
        {
            itemDetailsController = GameObject.Instantiate<ItemDetailsController>(itemDetailsPrefab, parent);
            itemCountSetterController = GameObject.Instantiate<ItemCountSetterController>(itemCountSetterPrefab, parent);
            confirmationRequestController = GameObject.Instantiate<ConfirmationRequestController>(confirmationRequestPrefab, parent);
            transactionCompleteController = GameObject.Instantiate<TransactionCompleteController>(transactionCompletePrefab, parent);

            SubscribeToEvents();
        }

        ~PopupService()
        {
            UnSubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GameService.Instance.EventService.OnItemSelected.AddListener(ShowItemDetails);
            GameService.Instance.EventService.OnItemActionInitiated.AddListener(ShowItemCountSetter);
            GameService.Instance.EventService.OnItemCountSet.AddListener(ShowConfirmationRequest);
            GameService.Instance.EventService.OnItemActionConfirmed.AddListener(ShowTransactionComepleteOverlay);
            GameService.Instance.EventService.OnTransactionCompleted.AddListener(HideIntermediatePopups);
        }

        private void UnSubscribeToEvents()
        {
            GameService.Instance.EventService.OnItemSelected.RemoveListener(ShowItemDetails);
            GameService.Instance.EventService.OnItemActionInitiated.RemoveListener(ShowItemCountSetter);
            GameService.Instance.EventService.OnItemCountSet.RemoveListener(ShowConfirmationRequest);
            GameService.Instance.EventService.OnItemActionConfirmed.RemoveListener(ShowTransactionComepleteOverlay);
            GameService.Instance.EventService.OnTransactionCompleted.RemoveListener(HideIntermediatePopups);
        }

        private void ShowItemDetails(ItemScriptableObject itemSO)
        {
            itemDetailsController.gameObject.SetActive(true);
            itemDetailsController.UpdateDetails(itemSO);
        }

        private void ShowItemCountSetter(ItemScriptableObject itemSO)
        {
            itemCountSetterController.gameObject.SetActive(true);
            itemCountSetterController.UpdateDetails(itemSO);
        }

        private void ShowConfirmationRequest(ItemScriptableObject itemSO, int count)
        {
            confirmationRequestController.gameObject.SetActive(true);
            confirmationRequestController.UpdateDetails(itemSO, count);
        }

        private void ShowTransactionComepleteOverlay(ItemScriptableObject itemSO)
        {
            transactionCompleteController.gameObject.SetActive(true);
            transactionCompleteController.DisplayOverlay(itemSO);
        }

        private void HideIntermediatePopups()
        {
            itemDetailsController.gameObject.SetActive(false);
            itemCountSetterController.gameObject.SetActive(false);
            confirmationRequestController.gameObject.SetActive(false);
        }
    }
}