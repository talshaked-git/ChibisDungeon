using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Chibis and Dungeons/Item/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private Item[] items;

    public Item GetItemRefference(string id)
    {
        foreach (Item item in items)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public Item GetItemCopy(string id)
    {
        try
        {
            Item item = GetItemRefference(id);
            if (item == null) return null;
            return item.GetCopy();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

}
