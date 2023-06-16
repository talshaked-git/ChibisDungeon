using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class IAPTemplate : MonoBehaviour, IDetailedStoreListener
{
    [SerializeField]
    private UIProduct UIProductPrefab;
    [SerializeField]
    private HorizontalLayoutGroup ContentPanel;
    [SerializeField]
    private GameObject LoadingOverLay;
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;
    private Action OnPurchaseCompeleted;
    private async void Awake() 
    {
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        .SetEnvironmentName("test");
#else
        .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoaded;
    }
    
    private void HandleIAPCatalogLoaded(AsyncOperation Operation)
    {
        ResourceRequest request = Operation as ResourceRequest;
        Debug.Log($"Loaded Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;
#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else 
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif
        foreach(ProductCatalogItem item in catalog.allProducts)
        {
            PayoutDefinition payout = new PayoutDefinition(item.Payouts[0].type.ToString(), item.Payouts[0].quantity);    
            Debug.Log("this is item payouts" +item.Payouts[0].ToString());
            builder.AddProduct(item.id, item.type, null ,payout);
        }
        UnityPurchasing.Initialize(this, builder);
    }
    
    public void PurchaseItem()
    {
        Debug.Log("purchase button clicked");
    }

    public void GrantChibiCoins(int chibis)
    {
        ChibiCoinsManager.instance.AddChibiCoins(chibis);
        PlayerManager.instance.SaveGame();
        Debug.Log("You received " + chibis + " Coins!");
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because of {failureDescription}.");
        OnPurchaseCompeleted?.Invoke();
        OnPurchaseCompeleted = null;
        LoadingOverLay.SetActive(false);
        //show purchase failed message to user
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"Error initializing IAP because of {error}." + $"\r\nShow a message to the player depending on the error.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"Failed to purchase because of {message}.");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}.");
        OnPurchaseCompeleted?.Invoke();
        OnPurchaseCompeleted = null;
        LoadingOverLay.SetActive(false);
        ChibiCoinsManager.instance.AddChibiCoins((int)purchaseEvent.purchasedProduct.definition.payout.quantity);
        PlayerManager.instance.SaveGame();
        return PurchaseProcessingResult.Complete;

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because of {failureReason}.");
        OnPurchaseCompeleted?.Invoke();
        OnPurchaseCompeleted = null;
        LoadingOverLay.SetActive(false);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        ExtensionProvider = extensions;
        StoreIconProvider.Initialize(controller.products);
        StoreIconProvider.OnLoadComplete += HandleAllIconsLoaded;


    }

    private void HandleAllIconsLoaded()
    {
        StartCoroutine(CreateUI());
        
    }

    private IEnumerator CreateUI()
    {
        List<Product> sortedProducts = StoreController.products.all.OrderBy(Item => Item.metadata.localizedPrice).ToList();
        //add before OrderBy .TakeWhile(item => !item.definition.id.Contains("sale"))

        foreach(Product product in sortedProducts)
        {
            UIProduct uiProduct = Instantiate(UIProductPrefab);
            uiProduct.OnPurchase += HandlePurchase;
            uiProduct.Setup(product);
            uiProduct.transform.SetParent(ContentPanel.transform, false);

            yield return null;
        }
    }

    private void HandlePurchase(Product Product, Action OnPurchaseComplete)
    {
        LoadingOverLay.SetActive(true);
        this.OnPurchaseCompeleted = OnPurchaseComplete;
        StoreController.InitiatePurchase(Product);
    }
}
