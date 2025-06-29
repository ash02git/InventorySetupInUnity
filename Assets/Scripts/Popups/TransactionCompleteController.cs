using System.Collections;
using TMPro;
using UnityEngine;

public class TransactionCompleteController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI overlayText;
    [SerializeField] private AudioSource transactionSound;
    [SerializeField] private AudioClip sellSound;
    [SerializeField] private AudioClip buySound;

    [SerializeField] private float overlayDisplayTime;

    public void DisplayOverlay(ItemScriptableObject itemSO)
    {
        overlayText.text = "You ";

        if (itemSO.doableAction == DoableAction.Buy)
        {
            overlayText.text += "bought ";
            if (buySound != null)
                transactionSound.clip = buySound;
        }
        else
        {
            overlayText.text += "sold ";
            if (sellSound != null)
                transactionSound.clip = sellSound;
        }
        overlayText.text += itemSO.id.ToString();

        StartCoroutine(overlayTimer());
    }

    private IEnumerator overlayTimer()
    {
        if (transactionSound != null)
            transactionSound.Play();

        yield return new WaitForSeconds(overlayDisplayTime);
        gameObject.SetActive(false);
    }
}
