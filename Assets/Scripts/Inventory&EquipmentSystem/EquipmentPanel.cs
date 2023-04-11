
using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<Item> OnItemPressEvent;

    private void Start() {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].OnPressEvent += OnItemPressEvent;
            equipmentSlots[i].OnTooltipActiveChanged += HandleTooltipActiveChangedEvent;
        }
    }

    private void OnValidate() {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem) {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            if (equipmentSlots[i].equipmentType == item.equipmentType) {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                equipmentSlots[i].item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item) {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            if (equipmentSlots[i].item == item) {
                equipmentSlots[i].item = null;
                equipmentSlots[i].icon.sprite = equipmentSlots[i].defualtIcon;
                equipmentSlots[i].icon.enabled = true;
                
                return true;
            }
        }
        return false;
    }

    // Add a method to get equipmentSlots
    public EquipmentSlot[] GetEquipmentSlots() {
        return equipmentSlots;
    }

    void HandleTooltipActiveChangedEvent(bool isActive, InventorySlot Excludeslot) {
        foreach (InventorySlot slot in equipmentSlots) {
            if (slot != Excludeslot) {
                slot.isCurrentTooltipActive = isActive;
            }
        }
    }

}
