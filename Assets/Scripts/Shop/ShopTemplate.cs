using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using System;

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
        costTxt.text = shop_item.baseCost.ToString();
        PackIcon.sprite = shop_item.PackIcon;
        
    }




    private void CreateDescription()
    {
        // Create a dictionary to hold the item counts
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        // Count the items
        foreach (Item item in shop_item.items)
        {
            if(itemCounts.ContainsKey(item.ItemName))
            {
                itemCounts[item.ItemName]++;
            }
            else
            {
                itemCounts.Add(item.ItemName, 1);
            }
        }

        // Clear the StringBuilder
        sb.Length = 0;

        // Generate the description
        foreach (KeyValuePair<string, int> pair in itemCounts)
        {
            if (pair.Value > 1)
            {
                sb.AppendLine(pair.Key + " x" + pair.Value);
            }
            else
            {
                sb.AppendLine(pair.Key);
            }
        }

        descriptionTxt.text = sb.ToString();
    }


    public void PurchasePack()
    {
        ShopManager.instance.PurchasePack(shop_item);
    }

}
