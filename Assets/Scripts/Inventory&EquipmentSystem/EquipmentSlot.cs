
using UnityEngine;


public class EquipmentSlot : InventorySlot
{
    public EquipmentType equipmentType;
    public Sprite defualtIcon;

    protected override void OnValidate()
    {
        base.OnValidate();
        string equipSlotName = equipmentType.ToString();
        gameObject.name = equipSlotName + " Slot";
    }

    public override bool CanReciveItem(Item item)
    {
        if (item == null)
        {
            return true;
        }

        EquippableItem equippableItem = item as EquippableItem;
        return equippableItem != null && equippableItem.equipmentType == equipmentType;
    }
}