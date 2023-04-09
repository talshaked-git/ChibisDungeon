
using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<Item> OnItemPressEvent;

    private void Awake() {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].OnPressEvent += OnItemPressEvent;
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
}
