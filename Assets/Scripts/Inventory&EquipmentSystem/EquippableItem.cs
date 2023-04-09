using UnityEngine;

public enum EquipmentType {
    Helmet,
    Chest,
    Gloves,
    Boots,
    Weapon_MainHand,
    Weapon_OffHand,
    Ring1,
    Ring2,
    Neckless, 
}


[CreateAssetMenu(fileName = "EquippableItem", menuName = "Chibis and Dungeons/EquippableItem", order = 0)]
public class EquippableItem : Item
{
    public int STRBonus;
    public int INTBonus;
    public int AGIBonus;
    public int VITBonus;
    [Space]
    public float STRPercentBonus;
    public float INTPercentBonus;
    public float AGIPercentBonus;
    public float VITPercentBonus;
    [Space]
    public EquipmentType equipmentType;
    
}
