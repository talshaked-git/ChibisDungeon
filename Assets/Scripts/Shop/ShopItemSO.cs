using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pack", menuName = "Premium Shop/New Shop Pack", order = 1)]
public class ShopItemSO : ScriptableObject 
{
    public string title;
    public string description;
    public int baseCost;
    public Sprite PackIcon;
    public Item[] items;
}

