using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IItemContainer
{
    [SerializeField] List<Item> startingItems;
    [SerializeField] Transform itemsParent;
    [SerializeField] List<InventorySlot> inventorySlots;
    [SerializeField] InventorySlot invSlotPrefab;
    int MaxSlots = 25;

    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    private void Start()
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


        SetStartingItems();
    }


    private void SetStartingItems()
    {
        Clear();

        for (int i = 0; i < startingItems.Count && i < inventorySlots.Count; i++)
        {
            inventorySlots[i].item = startingItems[i].GetCopy();
            inventorySlots[i].Amount = 1;
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
        }
    }
}
