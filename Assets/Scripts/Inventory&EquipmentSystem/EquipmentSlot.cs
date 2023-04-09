
using UnityEngine;

public class EquipmentSlot : InventorySlot
{
    public EquipmentType equipmentType;
    public Sprite defualtIcon;

     protected override void OnValidate(){
        base.OnValidate();
        gameObject.name = equipmentType.ToString() + " Slot";
     }

}
