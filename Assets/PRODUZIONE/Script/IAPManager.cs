using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class IAPManager : MonoBehaviour
{
    public GameObject buyButton, acquistatoButton,restoreButton;
    PlayerPrefsHandler playerPrefs;

    public void OnPurchaseComplete(Product product)
    {
        print("ok");
        StartCoroutine(disableButton());
        playerPrefs = new PlayerPrefsHandler();
        playerPrefs.SetRemoveAds(true);


    }

    public void OnPurchaseFailure(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Debug.Log("Purchased of " + product.definition.id + " failed due to " + purchaseFailureReason);
    }


    private IEnumerator disableButton()
    {
        yield return new WaitForEndOfFrame();
   
        buyButton.SetActive(false);
        restoreButton.GetComponent<Button>().interactable = false;
        acquistatoButton.SetActive(true);
    }
}
