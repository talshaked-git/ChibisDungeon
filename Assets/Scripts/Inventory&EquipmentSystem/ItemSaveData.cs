using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotSaveData
{
    public Item item;
    public int amount;

    public InventorySlotSaveData(InventorySlot slot)
    {
        item = slot.item;
        amount = slot.Amount;
    }

    public InventorySlotSaveData(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        var data = new Dictionary<string, System.Object>();
        //data.Add("item", item.ToDictionary());


        Type itemType = item.GetType();
        switch (itemType.Name)
        {
            case "EquippableItem":
                data.Add("ItemClass", "EquipableItem");
                break;
            case "UsableItem":
                data.Add("ItemClass", "UsableItem");
                break;
            default:
                data.Add("ItemClass", "Item");
                break;
        }

        data.Add("amount", amount);
        return data;
    }
}

public class InventorySaveData
{
    public const string NullPlaceholder = "NULL_PLACEHOLDER";
    public InventorySlotSaveData[] SavedSlots;
    private int numItems;

    public InventorySaveData(int numItems)
    {
        this.numItems = numItems;
        SavedSlots = new InventorySlotSaveData[numItems];
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> data = new Dictionary<string, System.Object>();
        data.Add("numItems", numItems);
        List<System.Object> savedSlots = new List<System.Object>();
        for (int i = 0; i < SavedSlots.Length; i++)
        {
            if (SavedSlots[i] == null)
            {
                savedSlots.Add(NullPlaceholder);
            }
            else
            {
                savedSlots.Add(SavedSlots[i].ToDictionary());
            }
        }
        data.Add("savedSlots", savedSlots);
        return data;
    }
}