using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    private void Awake() {
        inventory.OnItemPressEvent += EquipFromInventory;
        equipmentPanel.OnItemPressEvent += UnequipFromEquipmentPanel;
    }

    private void UnequipFromEquipmentPanel(Item item)
    {
        Unequip((EquippableItem)item);
    }

    private void EquipFromInventory(Item item) {
        if (item is EquippableItem) {
            Equip((EquippableItem)item);
        }
    }
    


    public void Equip(EquippableItem item) {
        if (inventory.RemoveItem(item)) {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem)) {
                if (previousItem != null) {
                    inventory.AddItem(previousItem);
                }
            } else {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item) {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item)) {
            inventory.AddItem(item);
        }
    }
}
