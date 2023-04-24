using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IItemContainer
{
    [SerializeField] Transform itemsParent;
    [SerializeField] List<InventorySlot> inventorySlots;
    [SerializeField] InventorySlot invSlotPrefab;
    public int MaxSlots { get; set; }

    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    public void InitInventory()
    {
        inventorySlots = new List<InventorySlot>(new InventorySlot[MaxSlots]);

        for (int i = 0; i < MaxSlots; i++)
        {
            inventorySlots[i] = Instantiate(invSlotPrefab, itemsParent);
        }

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            inventorySlots[i].OnPressEvent += OnPressEvent;
            inventorySlots[i].OnBeginDragEvent += OnBeginDragEvent;
            inventorySlots[i].OnEndDragEvent += OnEndDragEvent;
            inventorySlots[i].OnDragEvent += OnDragEvent;
            inventorySlots[i].OnDropEvent += OnDropEvent;
        }
        Clear();
    }

    public virtual bool CanAddItem(Item item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.item == null || slot.item.ID == item.ID)
            {
                freeSpaces += item.MaxStack - slot.Amount;
            }
        }

        return freeSpaces >= amount;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            if (inventorySlots[i].CanAddStack(item))
            {
                inventorySlots[i].Amount++;
                return true;
            }
        }
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            if (inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                inventorySlots[i].Amount++;
                return true;
            }
        }
        return false;
    }

    public void LoadInventory(InventorySaveData saveData)
    {
        Debug.Log(saveData);
        if (saveData == null) return;
        int index = 0;
        foreach (InventorySlotSaveData slot in saveData.SavedSlots)
        {
            if (slot == null) continue;
            if (slot.item == null) continue;
            inventorySlots[index].item = slot.item;
            inventorySlots[index].Amount = slot.amount;
            index++;
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots;
    }

    public void ResetIsTooltipActive(InventorySlot inventorySlot)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot != inventorySlot)
                slot.isTooltipActive = false;
        }
    }

    public bool ContainsItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            if (inventorySlots[i].item == item)
            {
                return true;
            }
        }
        return false;
    }

    public int ItemCount(string itemID)
    {
        int count = 0;
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            if (inventorySlots[i].item.ID == itemID)
            {
                count += inventorySlots[i].Amount;
            }
        }
        return count;
    }

    public Item RemoveItem(string itemID)
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            Item item = inventorySlots[i].item;
            if (item != null && item.ID == itemID)
            {
                inventorySlots[i].Amount--;
                return item;
            }
        }
        return null;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].Amount--;
                return true;
            }
        }
        return false;
    }

    public virtual void Clear()
    {
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            inventorySlots[i].item = null;
            inventorySlots[i].Amount = 0;
        }
    }
}
