
using System;
using System.Collections.Generic;

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
        set
        {
            _level = value;
            LevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private long _CurrentExp;
    public long CurrentExp
    {
        get { return _CurrentExp; }
        set
        {
            _CurrentExp = value;
            if (_CurrentExp >= requiredExpForNextLevel)
            {
                LevelUp();
            }
        }

    }
    private long _requiredExpForNextLevel;
    public long requiredExpForNextLevel
    {
        get { return _requiredExpForNextLevel; }
        set { _requiredExpForNextLevel = value; }
    }
    public int gold { get; set; }
    public CharcterStat HP { get; set; }
    public int currentHP { get; set; }
    public CharcterStat MP { get; set; }
    public CharcterStat STR { get; set; }
    public CharcterStat VIT { get; set; }
    public CharcterStat INT { get; set; }
    public CharcterStat AGI { get; set; }
    public CharcterStat DMG { get; set; }
    public CharcterStat DEF { get; set; }


    public event EventHandler LevelChanged;


    public Player(string _name, string _CID, CharClassType _class)
    {
        CID = _CID;
        name = _name;
        classType = _class;
        level = 1;
        CurrentExp = 0;
        gold = 100;
        _requiredExpForNextLevel = CalculateExpForLevel(level);
        LevelChanged += UpdateRequiredExpForNextLevel;
        // inventory = new List<int>();
        // equipment = new List<int>();
        InitStatesByClassType();
    }

    public Player(Dictionary<string, Object> _dictionary)
    {
        CID = _dictionary["CID"].ToString();
        name = _dictionary["name"].ToString();
        classType = (CharClassType)Convert.ToInt32(_dictionary["classType"]);
        level = Convert.ToInt32(_dictionary["level"]);
        CurrentExp = Convert.ToInt64(_dictionary["CurrentExp"]);
        gold = Convert.ToInt32(_dictionary["gold"]);
        // inventory = (List<int>)_dictionary["inventory"]; //TODO: fix this
        // equipment = (List<int>)_dictionary["equipment"]; //TODO: fix this
        HP = new CharcterStat((Dictionary<string, Object>)_dictionary["HP"]);
        MP = new CharcterStat((Dictionary<string, Object>)_dictionary["MP"]);
        STR = new CharcterStat((Dictionary<string, Object>)_dictionary["STR"]);
        VIT = new CharcterStat((Dictionary<string, Object>)_dictionary["VIT"]);
        INT = new CharcterStat((Dictionary<string, Object>)_dictionary["INT"]);
        AGI = new CharcterStat((Dictionary<string, Object>)_dictionary["AGI"]);
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
    }

    public virtual Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["CID"] = CID;
        result["name"] = name;
        result["classType"] = ((int)classType);
        result["level"] = level;
        result["CurrentExp"] = CurrentExp;
        result["gold"] = gold;
        // result["inventory"] = inventory;
        // result["equipment"] = equipment;
        result["HP"] = HP.ToDictionary();
        result["MP"] = MP.ToDictionary();
        result["STR"] = STR.ToDictionary();
        result["VIT"] = VIT.ToDictionary();
        result["INT"] = INT.ToDictionary();
        result["AGI"] = AGI.ToDictionary();

        return result;
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

    private void UpdateRequiredExpForNextLevel(object sender, EventArgs e)
    {
        _requiredExpForNextLevel = CalculateExpForLevel(level);
    }

    private void LevelUp()
    {
        _CurrentExp -= requiredExpForNextLevel;
        level++;
    }
}
