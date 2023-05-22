using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public PurchaseItemDialog purchaseItemDialog;
    public TMP_Text coinsUI;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        coinsUI.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    public void AddCoins()
    {
        PlayerManager.instance.CurrentPlayer.chibiCoins++;
        coinsUI.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    public void PurchasePack(ShopItemSO pack)
    {
        if (PlayerManager.instance.CurrentPlayer.chibiCoins < pack.baseCost)
        {
            purchaseItemDialog.SetText("Not enough Chibi Coins");
            purchaseItemDialog.Show();
            return;
        }

        PlayerManager.instance.CurrentPlayer.chibiCoins -= pack.baseCost;
        coinsUI.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
        //add items to inventory
        foreach (var item in pack.items)
        {
            if(!PlayerManager.instance.AddItem(item.GetCopy()))
            {
                //add mail system
                Debug.Log("inventory full - sent to mail");
            }
            Debug.Log("Added " + item.ItemName + " to inventory");
            
        }
        //save player
        PlayerManager.instance.SaveGame();
        purchaseItemDialog.SetText("Purchase Successful");
        purchaseItemDialog.Show();

    }

    
}
