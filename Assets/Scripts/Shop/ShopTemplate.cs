using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    public TMP_Text titleTxt;
    public TMP_Text descriptionTxt;
    public TMP_Text costTxt;

    public Image PackIcon;

    public Button purchaseBtn;
    public ShopItemSO shop_item;
    protected static readonly StringBuilder sb = new StringBuilder();

    private void Start()
    {
        titleTxt.text = shop_item.title;
        CreateDescription();
        costTxt.text = shop_item.baseCost.ToString() + " Chibi Coins";
        PackIcon.sprite = shop_item.PackIcon;
        
    }

    private void CreateDescription()
    {
        
        sb.Length = 0;
        foreach (Item item in shop_item.items)
        {
            sb.AppendLine(item.ItemName);
        }
        descriptionTxt.text = sb.ToString();

    }

    public void PurchasePack()
    {
        ShopManager.instance.PurchasePack(shop_item);
    }

}
