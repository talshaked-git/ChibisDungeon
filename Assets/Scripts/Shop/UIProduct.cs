using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIProduct : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private Button purchaseButton;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);
    public event PurchaseEvent OnPurchase;

    private Product Model;

    public void Setup(Product Product)
    {
        Model = Product;
        titleText.SetText(Product.metadata.localizedTitle);
        descriptionText.SetText(Product.metadata.localizedDescription);
        priceText.SetText($"{Product.metadata.localizedPriceString} " + Product.metadata.isoCurrencyCode);
        Texture2D texture = StoreIconProvider.GetIcon(Product.definition.id);
        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
            Icon.sprite = sprite;
        }
        else
        {
            Debug.LogError($"Failed to load icon for product {Product.definition.id}");
        }
    }

    public void Purchase()
    {
        purchaseButton.enabled = false;
        OnPurchase?.Invoke(Model, HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        purchaseButton.enabled = true;
    }
}
