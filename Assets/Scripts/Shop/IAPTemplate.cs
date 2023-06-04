using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPTemplate : MonoBehaviour
{

    public void PurchaseItem()
    {
        Debug.Log("purchase button clicked");
    }

    public void GrantChibiCoins(int chibis)
    {
        ChibiCoinsManager.instance.AddChibiCoins(chibis);
        Debug.Log("You received " + chibis + " Coins!");
        PlayerManager.instance.SaveGame();
    }

    public void PurchaseFailed(Product product, UnityEngine.Purchasing.Extension.PurchaseFailureDescription failureReason)
    {
        try
        {
            Debug.Log("Purchase Failed");
            Debug.Log("Product ID: " + product.definition.id);
            Debug.Log("Purchase Failure Reason: " + failureReason);
        }
        catch (Exception e)
        {
            Debug.Log("Purchase Failed");
            Debug.Log("Purchase Failure Reason: " + failureReason);
        }
        Debug.Log("Purchasing Unavailable :\n" + failureReason);
        Debug.Log("Purchase Failed");
    }
}
