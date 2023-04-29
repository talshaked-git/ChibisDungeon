
using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<InventorySlot> OnPressEvent;
    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    private void Start()
    {
        if (equipmentSlotsParent == null)
        {
            equipmentSlotsParent = GetComponent<Transform>();
        }

        if (equipmentSlotsParent == null)
        {
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPressEvent += OnPressEvent;
            equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            equipmentSlots[i].OnDragEvent += OnDragEvent;
            equipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].equipmentType == item.equipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                equipmentSlots[i].item = item;
                equipmentSlots[i].Amount = 1;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].item == item)
            {
                equipmentSlots[i].item = null;
                equipmentSlots[i].icon.sprite = equipmentSlots[i].defualtIcon;
                equipmentSlots[i].Amount = 0;
                return true;
            }
        }
        return false;
    }

    // Add a method to get equipmentSlots
    public EquipmentSlot[] GetEquipmentSlots()
    {
        return equipmentSlots;
    }

    public void ResetIsTooltipActive(InventorySlot inventorySlot)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] == inventorySlot)
            {
                equipmentSlots[i].isTooltipActive = false;
            }
        }
    }
}
