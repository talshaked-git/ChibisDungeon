using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;

public class ShopManager : MonoBehaviour, IStoreListener
{
    public static ShopManager instance;
    public PurchaseItemDialog purchaseItemDialog;
    public TMP_Text coinsUI;

    public GameObject premiumPacksPanel, chibiCoinsPanel, goldPacksPanel, rewardsPanel;
    private GameObject currentPanel;
    
    private IStoreController controller;
    private IExtensionProvider extensions;
    
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
        ChibiCoinsManager.instance.OnChibiCoinsChange += UpdateCoinsText;
        UpdateCoinsText();
        
        // Start Unity IAP initialization
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        // Add your products with their IDs
        builder.AddProduct("product1", ProductType.Consumable);
        builder.AddProduct("product2", ProductType.NonConsumable);
        //...add more products if needed

        // Initialize with this class as the listener
        UnityPurchasing.Initialize(this, builder);

    }

    public void AddCoins()
    {
        ChibiCoinsManager.instance.AddChibiCoins(20);
    }

    private void UpdateCoinsText()
    {
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
        if (!ChibiCoinsManager.instance.RemoveChibiCoins(pack.baseCost))
        {
            purchaseItemDialog.SetText("Not enough Chibi Coins");
            purchaseItemDialog.Show();
            return;
        }

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

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Store the references of controller and extensions
        this.controller = controller;
        this.extensions = extensions;

        Debug.Log("OnInitialized: PASS");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("OnInitializeFailed: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchase Successful: " + e.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("OnInitializeFailed: " + error + ", Message: " + message);
    }
}
