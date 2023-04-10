using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot[] inventorySlots;

    public event Action<Item> OnItemPressEvent;

    private void Awake() {
        for (int i = 0; i < inventorySlots.Length; i++) {
            inventorySlots[i].OnPressEvent += OnItemPressEvent;
        }
    }

    private void OnValidate() {
        if (itemsParent != null) {
            inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }

        RefreshUI();
    }

    private void RefreshUI(){
        int i = 0;
        for(; i < items.Count && i < inventorySlots.Length; i++){
            inventorySlots[i].item = items[i];
        }

        for(; i < inventorySlots.Length; i++){
            inventorySlots[i].item = null;
        }
    }

    public bool AddItem(Item item){
        if(IsFull())
            return false;

        items.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Item item){
        if(items.Remove(item)){
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        return items.Count >= inventorySlots.Length;
    }

    // Add a method to get inventorySlots
    public InventorySlot[] GetInventorySlots() {
        return inventorySlots;
    }
}
