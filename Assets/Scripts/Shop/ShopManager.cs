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

    public GameObject premiumPacksPanel, chibiCoinsPanel, goldPacksPanel, rewardsPanel;
    private GameObject currentPanel;
    
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
        currentPanel = premiumPacksPanel;
        currentPanel.SetActive(true);
        chibiCoinsPanel.SetActive(false);
        goldPacksPanel.SetActive(false);
        rewardsPanel.SetActive(false);
        coinsUI.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    public void AddCoins()
    {
        PlayerManager.instance.CurrentPlayer.chibiCoins+= 20;
        coinsUI.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    public void ShowPremiumPacks()
    {
        if (currentPanel != premiumPacksPanel)
        {
            currentPanel.SetActive(false);
            premiumPacksPanel.SetActive(true);
            currentPanel = premiumPacksPanel;
        }
    }

    public void ShowChibiCoins()
    {
        if (currentPanel != chibiCoinsPanel)
        {
            currentPanel.SetActive(false);
            chibiCoinsPanel.SetActive(true);
            currentPanel = chibiCoinsPanel;
        }
    }

    public void ShowGoldPacks()
    {
        if (currentPanel != goldPacksPanel)
        {
            currentPanel.SetActive(false);
            goldPacksPanel.SetActive(true);
            currentPanel = goldPacksPanel;
        }
    }

    public void ShowRewards()
    {
        if (currentPanel != rewardsPanel)
        {
            currentPanel.SetActive(false);
            rewardsPanel.SetActive(true);
            currentPanel = rewardsPanel;
        }
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
