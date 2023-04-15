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
    [SerializeField] InventorySlot[] inventorySlots;

    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].OnPressEvent += OnPressEvent;
            inventorySlots[i].OnBeginDragEvent += OnBeginDragEvent;
            inventorySlots[i].OnEndDragEvent += OnEndDragEvent;
            inventorySlots[i].OnDragEvent += OnDragEvent;
            inventorySlots[i].OnDropEvent += OnDropEvent;
        }

        
        SetStartingItems();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }

        SetStartingItems();
    }

    private void SetStartingItems()
    {
        int i = 0;
        for (; i < startingItems.Count && i < inventorySlots.Length; i++)
        {
            inventorySlots[i].item = Instantiate(startingItems[i]);
        }

        for (; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].item = null;
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == item)
            {
                inventorySlots[i].item = null;
                return true;
            }
        }
        return false;
    }

    public bool IsFull()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == null)
            {
                return false;
            }
        }
        return true;
    }

    public InventorySlot[] GetInventorySlots()
    {
        return inventorySlots;
    }

    public void ResetIsTooltipActive(InventorySlot inventorySlot)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != inventorySlot)
                inventorySlots[i].isTooltipActive = false;
        }
    }

    public bool ContainsItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
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
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item.ID == itemID)
            {
                count++;
            }
        }
        return count;
    }

    public Item RemoveItem(string itemID)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Item item = inventorySlots[i].item;
            if (item != null && item.ID == itemID)
            {
                inventorySlots[i].item = null;
                return item;
            }
        }
        return null;
    }
}
