using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : ItemContainer, IItemContainer
{
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot invSlotPrefab;
    public int MaxSlots { get; set; }


    public void InitInventory()
    {
        this.inventorySlots = new List<InventorySlot>();

        for (int i = 0; i < MaxSlots; i++)
        {
            inventorySlots.Add(Instantiate(invSlotPrefab, itemsParent));
        }
        Clear();
        InitContainer();
    }

    public void LoadInventory(InventorySaveData saveData)
    {
        Debug.Log(saveData);
        if (saveData == null) return;
        int index = 0;
        foreach (InventorySlotSaveData slot in saveData.SavedSlots)
        {
            
            if (slot == null) continue;
            if (slot.itemSaveData == null) continue;
            Item item = slot.itemSaveData.ToItem();
            inventorySlots[index].item = item;
            inventorySlots[index].Amount = slot.amount;
            index++;
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots;
    }

}
