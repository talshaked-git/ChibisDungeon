
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharClassType
{
    Archer = 1000,
    Wizard = 2000,
    Warrior = 3000,
    Rogue = 4000,
    Any = 0
}


[System.Serializable]
public class Player
{
    public static int LevelFactor1 = 50;
    public static int LevelFactor2 = 100;
    public string CID { get; set; }
    public string name { get; set; }
    public CharClassType classType { get; set; }
    private int _level;
    public int level
    {
        get { return _level; }
        set { _level = value; }
    }
    private long m_currentExp;
    public long CurrentExp
    {
        get { return m_currentExp; }
        private set { m_currentExp = value; }
    }
    private long _requiredExpForNextLevel;
    public long requiredExpForNextLevel
    {
        get { return _requiredExpForNextLevel; }
        set { _requiredExpForNextLevel = value; }
    }
    private int m_attributePoints;
    public int AttributePoints
    {
        get { return m_attributePoints; }
        private set { m_attributePoints = value; }
    }

    public int gold { get; set; }
    public CharcterStat HP { get; set; }
    private int _currentHP;
    public int currentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = value;
            if (_currentHP <= 0)
            {
                // Die();
            }
        }
    }
    public CharcterStat MP { get; set; }
    public int currentMP { get; set; }
    public CharcterStat STR { get; set; }
    public CharcterStat VIT { get; set; }
    public CharcterStat INT { get; set; }
    public CharcterStat AGI { get; set; }
    public CharcterStat DMG { get; set; }
    public CharcterStat DEF { get; set; }

    public InventorySaveData InventorySaveData { get; set; }
    public InventorySaveData EquipmentSaveData { get; set; }

    private Dictionary<string, System.Object> InventorySaveDataDictionary;
    private Dictionary<string, System.Object> EquipmentSaveDataDictionary;


    public int InventoryMaxSlots { get; set; }


    public string LastLocation { get; set; }


    public Player(string _name, string _CID, CharClassType _class)
    {
        CID = _CID;
        name = _name;
        classType = _class;
        level = 1;
        CurrentExp = 0;
        gold = 100;
        AttributePoints = 0;
        _requiredExpForNextLevel = CalculateExpForLevel(level);
        InventoryMaxSlots = 20;
        InventorySaveData = new InventorySaveData(InventoryMaxSlots);
        EquipmentSaveData = new InventorySaveData(9);
        InitStatesByClassType();
        currentHP = (int)HP.Value;
        currentMP = (int)MP.Value;
        LastLocation = "Scene_Forest_Town";
    }

    public Player(Dictionary<string, System.Object> _dictionary)
    {
        CID = _dictionary["CID"].ToString();
        name = _dictionary["name"].ToString();
        classType = (CharClassType)Convert.ToInt32(_dictionary["classType"]);
        level = Convert.ToInt32(_dictionary["level"]);
        CurrentExp = Convert.ToInt64(_dictionary["CurrentExp"]);
        gold = Convert.ToInt32(_dictionary["gold"]);
        AttributePoints = Convert.ToInt32(_dictionary["AttributePoints"]);
        InventorySaveDataDictionary = (Dictionary<string, System.Object>)_dictionary["InventorySaveData"];
        EquipmentSaveDataDictionary = (Dictionary<string, System.Object>)_dictionary["EquipmentSaveData"];
        InventoryMaxSlots = Convert.ToInt32(_dictionary["InventoryMaxSlots"]);
        HP = new CharcterStat((Dictionary<string, System.Object>)_dictionary["HP"]);
        MP = new CharcterStat((Dictionary<string, System.Object>)_dictionary["MP"]);
        STR = new CharcterStat((Dictionary<string, System.Object>)_dictionary["STR"]);
        VIT = new CharcterStat((Dictionary<string, System.Object>)_dictionary["VIT"]);
        INT = new CharcterStat((Dictionary<string, System.Object>)_dictionary["INT"]);
        AGI = new CharcterStat((Dictionary<string, System.Object>)_dictionary["AGI"]);
        DMG = new CharcterStat(0);
        DEF = new CharcterStat(0);

        _requiredExpForNextLevel = CalculateExpForLevel(level);
        ListenAndUpdateDerivedStats();

        currentHP = (int)HP.Value;
        currentMP = (int)MP.Value;

        LastLocation = _dictionary["LastLocation"].ToString();

    }

    public void LoadeInventoryAndERquipment()
    {
        InventorySaveData = ItemSaveManager.Instance.LoadInventory(InventorySaveDataDictionary);
        EquipmentSaveData = ItemSaveManager.Instance.LoadEquipment(EquipmentSaveDataDictionary);
    }

    public virtual Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["CID"] = CID;
        result["name"] = name;
        result["classType"] = ((int)classType);
        result["level"] = level;
        result["CurrentExp"] = CurrentExp;
        result["gold"] = gold;
        result["AttributePoints"] = AttributePoints;
        result["InventorySaveData"] = InventorySaveData.ToDictionary();
        result["EquipmentSaveData"] = EquipmentSaveData.ToDictionary();
        result["InventoryMaxSlots"] = InventoryMaxSlots;
        result["HP"] = HP.ToDictionary();
        result["MP"] = MP.ToDictionary();
        result["STR"] = STR.ToDictionary();
        result["VIT"] = VIT.ToDictionary();
        result["INT"] = INT.ToDictionary();
        result["AGI"] = AGI.ToDictionary();
        result["LastLocation"] = LastLocation;

        return result;
    }

    private void InitStatesByClassType()
    {
        switch (classType)
        {
            case CharClassType.Archer:
                HP = new CharcterStat(50);
                MP = new CharcterStat(25);
                STR = new CharcterStat(10);
                VIT = new CharcterStat(15);
                INT = new CharcterStat(5);
                AGI = new CharcterStat(30);
                break;
            case CharClassType.Wizard:
                HP = new CharcterStat(30);
                MP = new CharcterStat(60);
                STR = new CharcterStat(5);
                VIT = new CharcterStat(10);
                INT = new CharcterStat(30);
                AGI = new CharcterStat(10);
                break;
            case CharClassType.Warrior:
                HP = new CharcterStat(80);
                MP = new CharcterStat(20);
                STR = new CharcterStat(30);
                VIT = new CharcterStat(20);
                INT = new CharcterStat(5);
                AGI = new CharcterStat(10);
                break;
            case CharClassType.Rogue:
                HP = new CharcterStat(40);
                MP = new CharcterStat(30);
                STR = new CharcterStat(10);
                VIT = new CharcterStat(15);
                INT = new CharcterStat(10);
                AGI = new CharcterStat(30);
                break;
            default:
                break;
        }
        DMG = new CharcterStat(0);
        DEF = new CharcterStat(0);

        ListenAndUpdateDerivedStats();
    }

    private void ListenAndUpdateDerivedStats()
    {
        STR.StatChanged += UpdateDerivedStats;
        VIT.StatChanged += UpdateDerivedStats;
        INT.StatChanged += UpdateDerivedStats;
        AGI.StatChanged += UpdateDerivedStats;

        UpdateDerivedStats();
    }

    private void UpdateDerivedStats()
    {
        switch (classType)
        {
            case CharClassType.Archer:
                HP.BaseValue = 50f + 5f * VIT.Value + 1f * STR.Value;
                MP.BaseValue = 25f + 5f * AGI.Value + 3f * INT.Value;
                DMG.BaseValue = 5f + 1.5f * AGI.Value + 0.5f * STR.Value;
                DEF.BaseValue = 10f + 2f * VIT.Value + 0.5f * AGI.Value;
                break;
            case CharClassType.Wizard:
                HP.BaseValue = 30f + 3f * VIT.Value + 1f * STR.Value;
                MP.BaseValue = 60f + 8f * INT.Value;
                DMG.BaseValue = 5f + 2f * INT.Value;
                DEF.BaseValue = 8f + 1f * VIT.Value + 0.5f * AGI.Value;
                break;
            case CharClassType.Warrior:
                HP.BaseValue = 80f + 10f * VIT.Value + 3f * STR.Value;
                MP.BaseValue = 20f + 4f * INT.Value;
                DMG.BaseValue = 10f + 1.5f * STR.Value + 0.5f * AGI.Value;
                DEF.BaseValue = 12f + 5f * VIT.Value + 0.5f * STR.Value;
                break;
            case CharClassType.Rogue:
                HP.BaseValue = 40f + 4f * VIT.Value + 2f * STR.Value;
                MP.BaseValue = 30f + 3f * AGI.Value + 3f * INT.Value;
                DMG.BaseValue = 7f + 1.5f * AGI.Value + 0.5f * STR.Value;
                DEF.BaseValue = 10f + 3f * VIT.Value + 1f * AGI.Value;
                break;
            default:
                break;
        }

        if (currentHP > HP.Value)
        {
            currentHP = (int)HP.Value;
        }
        if (currentMP > MP.Value)
        {
            currentMP = (int)MP.Value;
        }
    }

    //toString
    public override string ToString()
    {
        return "Player [name=" + name + ", classType=" + classType + ", level=" + level + ", exp=" + CurrentExp + ", gold=" + gold + ", HP=" + HP + ", MP=" + MP + ", STR=" + STR + ", VIT=" + VIT + ", INT=" + INT + ", AGI=" + AGI + "]";
    }

    private static long CalculateExpForLevel(int level)
    {
        // Example: Exponential and linear growth formula
        return (long)((Math.Pow(level, 2.1) * LevelFactor1) + (level * LevelFactor2));
    }

    public void AddExp(long exp)
    {
        CurrentExp += exp;
        if (CurrentExp >= _requiredExpForNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentExp -= _requiredExpForNextLevel;
        AttributePoints += 3;
        level++;
        _requiredExpForNextLevel = CalculateExpForLevel(level);
    }

    public bool UseAttributePoints(int points, string stat)
    {
        if (AttributePoints < points)
        {
            return false;
        }

        switch (stat)
        {
            case "STR":
                STR.BaseValue += points;
                break;
            case "INT":
                INT.BaseValue += points;
                break;
            case "VIT":
                VIT.BaseValue += points;
                break;
            case "AGI":
                AGI.BaseValue += points;
                break;
            default:
                break;
        }
        AttributePoints -= points;

        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

}
