using Firebase.Firestore;
using System;
using System.Collections.Generic;
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


[CreateAssetMenu(fileName = "EquippableItem", menuName = "Chibis and Dungeons/Item/Equippable Item")]
[FirestoreData]
public class EquippableItem : Item
{
    [SerializeField]
    private int _strBonus;
    [FirestoreProperty]
    public int STRBonus
    {
        get { return _strBonus; }
        set { _strBonus = value; }
    }

    [SerializeField]
    private int _intBonus;
    [FirestoreProperty]
    public int INTBonus
    {
        get { return _intBonus; }
        set { _intBonus = value; }
    }

    [SerializeField]
    private int _agiBonus;
    [FirestoreProperty]
    public int AGIBonus
    {
        get { return _agiBonus; }
        set { _agiBonus = value; }
    }

    [SerializeField]
    private int _vitBonus;
    [FirestoreProperty]
    public int VITBonus
    {
        get { return _vitBonus; }
        set { _vitBonus = value; }
    }

    [SerializeField]
    private float _strPercentAddBonus;
    [FirestoreProperty]
    public float STRPercentAddBonus
    {
        get { return _strPercentAddBonus; }
        set { _strPercentAddBonus = value; }
    }

    [SerializeField]
    private float _intPercentAddBonus;
    [FirestoreProperty]
    public float INTPercentAddBonus
    {
        get { return _intPercentAddBonus; }
        set { _intPercentAddBonus = value; }
    }

    [SerializeField]
    private float _vitPercentAddBonus;
    [FirestoreProperty]
    public float VITPercentAddBonus
    {
        get { return _vitPercentAddBonus; }
        set { _vitPercentAddBonus = value; }
    }

    [SerializeField]
    private float _agiPercentAddBonus;
    [FirestoreProperty]
    public float AGIPercentAddBonus
    {
        get { return _agiPercentAddBonus; }
        set { _agiPercentAddBonus = value; }
    }

    [SerializeField]
    private float _strPercentMullBonus;
    [FirestoreProperty]
    public float STRPercentMullBonus
    {
        get { return _strPercentMullBonus; }
        set { _strPercentMullBonus = value; }
    }

    [SerializeField]
    private float _intPercentMullBonus;
    [FirestoreProperty]
    public float INTPercentMullBonus
    {
        get { return _intPercentMullBonus; }
        set { _intPercentMullBonus = value; }
    }

    [SerializeField]
    private float _agiPercentMullBonus;
    [FirestoreProperty]
    public float AGIPercentMullBonus
    {
        get { return _agiPercentMullBonus; }
        set { _agiPercentMullBonus = value; }
    }

    [SerializeField]
    private float _vitPercentMullBonus;
    [FirestoreProperty]
    public float VITPercentMullBonus
    {
        get { return _vitPercentMullBonus; }
        set { _vitPercentMullBonus = value; }
    }

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

    override public string GetItemType()
    {
        return equipmentType.ToString();
    }

    override public string GetDescription()
    {
        sb.Length = 0;

        AddStat(STRBonus, "STR");
        AddStat(INTBonus, "INT");
        AddStat(VITBonus, "VIT");
        AddStat(AGIBonus, "AGI");
        AddStat(STRPercentAddBonus, "STR", true);
        AddStat(INTPercentAddBonus, "INT", true);
        AddStat(VITPercentAddBonus, "VIT", true);
        AddStat(AGIPercentAddBonus, "AGI", true);

        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercentage = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (value > 0)
            {
                sb.Append("+");
            }

            if (isPercentage)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }

            sb.Append(statName);
        }
    }

    public void UpdateStatValues()
    {

    }
}