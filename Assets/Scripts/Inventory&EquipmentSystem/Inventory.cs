using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot[] inventorySlots;

    public event Action<Item> OnItemPressEvent;

    private void Start() {
        for (int i = 0; i < inventorySlots.Length; i++) {
            inventorySlots[i].OnPressEvent += OnItemPressEvent;
            inventorySlots[i].OnTooltipActiveChanged += HandleTooltipActiveChangedEvent;
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

    void HandleTooltipActiveChangedEvent(bool isActive, InventorySlot Excludeslot) {
        foreach (InventorySlot slot in inventorySlots) {
            if (slot != Excludeslot) {
                slot.isCurrentTooltipActive = isActive;
            }
        }
    }
}
