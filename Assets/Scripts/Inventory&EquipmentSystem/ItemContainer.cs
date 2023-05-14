using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
{
    public List<InventorySlot> inventorySlots;

    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;


    public void InitContainer()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].OnPressEvent += slot =>EventHelper(slot,OnPressEvent);
            inventorySlots[i].OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
            inventorySlots[i].OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
            inventorySlots[i].OnDragEvent += slot => EventHelper(slot, OnDragEvent);
            inventorySlots[i].OnDropEvent +=  slot => EventHelper(slot, OnDropEvent);
        }
    }

    public void EventHelper(InventorySlot slot,Action<InventorySlot> action)
    {
        if (action != null)
        {
            action(slot);
        }
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

    public bool AddItem(Item item,int amount=1)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].CanAddStack(item,amount))
            {
                inventorySlots[i].Amount+=amount;
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                inventorySlots[i].Amount+=amount;
                return true;
            }
        }
        return false;
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
        for (int i = 0; i < inventorySlots.Count; i++)
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
        for (int i = 0; i < inventorySlots.Count; i++)
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
        for (int i = 0; i < inventorySlots.Count; i++)
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

    public bool RemoveItem(Item item,int amount=1)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == item && inventorySlots[i].Amount >= amount)
            {
                inventorySlots[i].Amount -= amount;
                return true;
            }
        }
        return false;
    }

    public virtual void Clear()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].item = null;
            inventorySlots[i].Amount = 0;
        }
    }

}
