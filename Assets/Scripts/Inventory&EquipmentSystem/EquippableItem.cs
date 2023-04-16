using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chest,
    Gloves,
    Boots,
    MainHand,
    OffHand,
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
    public float STRPercentAddBonus;
    public float INTPercentAddBonus;
    public float AGIPercentAddBonus;
    public float VITPercentAddBonus;
    [Space]
    public float STRPercentMullBonus;
    public float INTPercentMullBonus;
    public float AGIPercentMullBonus;
    public float VITPercentMullBonus;
    [Space]
    public EquipmentType equipmentType;

    public bool isEquipped = false;


    public void Equip(Player player)
    {
        if (STRBonus != 0)
        {
            player.STR.AddModifier(new StatModifier(STRBonus, StatModType.Flat, this));
        }
        if (INTBonus != 0)
        {
            player.INT.AddModifier(new StatModifier(INTBonus, StatModType.Flat, this));
        }
        if (AGIBonus != 0)
        {
            player.AGI.AddModifier(new StatModifier(AGIBonus, StatModType.Flat, this));
        }
        if (VITBonus != 0)
        {
            player.VIT.AddModifier(new StatModifier(VITBonus, StatModType.Flat, this));
        }
        if (STRPercentAddBonus != 0)
        {
            player.STR.AddModifier(new StatModifier(STRPercentAddBonus, StatModType.PercentMult, this));
        }
        if (INTPercentAddBonus != 0)
        {
            player.INT.AddModifier(new StatModifier(INTPercentAddBonus, StatModType.PercentMult, this));
        }
        if (AGIPercentAddBonus != 0)
        {
            player.AGI.AddModifier(new StatModifier(AGIPercentAddBonus, StatModType.PercentMult, this));
        }
        if (VITPercentAddBonus != 0)
        {
            player.VIT.AddModifier(new StatModifier(VITPercentAddBonus, StatModType.PercentMult, this));
        }
    }

    public void Unequip(Player player)
    {
        player.STR.RemoveAllModifiersFromSource(this);
        player.INT.RemoveAllModifiersFromSource(this);
        player.AGI.RemoveAllModifiersFromSource(this);
        player.VIT.RemoveAllModifiersFromSource(this);
    }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }
}