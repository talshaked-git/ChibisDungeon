
using UnityEngine;


public class EquipmentSlot : InventorySlot
{
    public EquipmentType equipmentType;
    public Sprite defualtIcon;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = equipmentType.ToString() + " Slot";
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