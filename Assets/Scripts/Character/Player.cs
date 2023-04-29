
using System;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public enum CharClassType
{
    Archer = 1000,
    Wizard = 2000,
    Warrior = 3000,
    Rogue = 4000,
    Any = 0
}

[FirestoreData]
public class Player
{
    public static int LevelFactor1 = 50;
    public static int LevelFactor2 = 100;


    [FirestoreProperty]
    public DocumentReference AccountRef { get; set; }

    [FirestoreDocumentId]
    public string CID { get; set; }

    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public CharClassType classType { get; set; }


    [FirestoreProperty]
    public int Level { get ; set; }

    [FirestoreProperty]
    public long CurrentExp { get; set; }

    private long _requiredExpForNextLevel;

    [FirestoreProperty]
    public long requiredExpForNextLevel
    {
        get { return _requiredExpForNextLevel; }
        set { _requiredExpForNextLevel = value; }
    }
    private int m_attributePoints;
    
    [FirestoreProperty]
    public int AttributePoints
    {
        get { return m_attributePoints; }
        set { m_attributePoints = value; }
    }

    [FirestoreProperty]
    public int gold { get; set; }

    [FirestoreProperty]
    public CharcterStat HP { get; set; }
    private int _currentHP;
    [FirestoreProperty]
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
    [FirestoreProperty]
    public CharcterStat MP { get; set; }
    [FirestoreProperty]
    public int currentMP { get; set; }

    [FirestoreProperty]
    public CharcterStat STR { get; set; }

    [FirestoreProperty]
    public CharcterStat VIT { get; set; }

    [FirestoreProperty]
    public CharcterStat INT { get; set; }

    [FirestoreProperty]
    public CharcterStat AGI { get; set; }

    [FirestoreProperty]
    public CharcterStat DMG { get; set; }

    [FirestoreProperty]
    public CharcterStat DEF { get; set; }

    [FirestoreProperty("Inventory")]
    public InventorySaveData InventorySaveData { get; set; } 
    [FirestoreProperty("Equipment")]
    public InventorySaveData EquipmentSaveData { get; set; }

    [FirestoreProperty]
    public int InventoryMaxSlots { get; set; }

    [FirestoreProperty]
    public string LastLocation { get; set; }

    public Player() { }

    public Player(string _name, string _CID, CharClassType _class)
    {
        CID = _CID;
        name = _name;
        classType = _class;
        Level = 1;
        CurrentExp = 0;
        gold = 100;
        AttributePoints = 0;
        _requiredExpForNextLevel = CalculateExpForLevel(Level);
        InventoryMaxSlots = 20;
        InventorySaveData = new InventorySaveData(InventoryMaxSlots);
        EquipmentSaveData = new InventorySaveData(9);
        InitStatesByClassType();
        currentHP = (int)HP.Value;
        currentMP = (int)MP.Value;
        LastLocation = "Scene_Forest_Town";
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

    public void ListenAndUpdateDerivedStats()
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
        return "Player [name=" + name + ", classType=" + classType + ", level=" + Level + ", exp=" + CurrentExp + ", gold=" + gold + ", HP=" + HP + ", MP=" + MP + ", STR=" + STR + ", VIT=" + VIT + ", INT=" + INT + ", AGI=" + AGI + "]";
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
        Level++;
        _requiredExpForNextLevel = CalculateExpForLevel(Level);
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
