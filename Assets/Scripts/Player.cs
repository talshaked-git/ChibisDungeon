
using System;
using System.Collections.Generic;

public enum CharClassType{
    Archer = 1000,
    Wizard = 2000,
    Warrior = 3000,
    Rogue = 4000,
}


[System.Serializable]
public class Player
{
    // public string CID { get; set; }
    public string name { get; set; }
    public CharClassType classType { get; set; }
    public int level { get; set; }
    public long exp { get; set; }
    public int gold { get; set; }
    public int[] inventory { get; set; } // change to inventory class To be made
    public int[] equipment { get; set; } // change to equipment class To be made
    public CharcterStat hp { get; set; }
    public CharcterStat mp { get; set; }
    public CharcterStat statSTR { get; set; }
    public CharcterStat statVIT { get; set; }
    public CharcterStat statINT { get; set; }
    public CharcterStat StatAGI { get; set; }

    public Player(string _name, CharClassType _class)
    {
        // CID = _CID;
        name = _name;
        classType = _class;
        level = 1;
        exp = 0;
        gold = 100;
        inventory = new int[20];
        equipment = new int[6];

    }

    public Player(Dictionary<string, Object> _dictionary)
    {
        // CID = _dictionary["CID"].ToString();
        name = _dictionary["name"].ToString();
        classType = (CharClassType)Convert.ToInt32(_dictionary["classType"]);
        level = Convert.ToInt32(_dictionary["level"]);
        exp = Convert.ToInt64(_dictionary["exp"]);
        gold = Convert.ToInt32(_dictionary["gold"]);
        inventory = (int[])_dictionary["inventory"];
        equipment = (int[])_dictionary["equipment"];
        hp = new CharcterStat((Dictionary<string, Object>)_dictionary["hp"]);
        mp = new CharcterStat((Dictionary<string, Object>)_dictionary["mp"]);
        statSTR = new CharcterStat((Dictionary<string, Object>)_dictionary["statSTR"]);
        statVIT = new CharcterStat((Dictionary<string, Object>)_dictionary["statVIT"]);
        statINT = new CharcterStat((Dictionary<string, Object>)_dictionary["statINT"]);
        StatAGI = new CharcterStat((Dictionary<string, Object>)_dictionary["StatAGI"]);
    }

    private void InitStatesByClassType(){
        switch (classType)
        {
            case CharClassType.Archer:        
                hp = new CharcterStat(250);
                mp = new CharcterStat(150);
                statSTR = new CharcterStat(15);
                statVIT = new CharcterStat(10);
                statINT = new CharcterStat(5);
                StatAGI = new CharcterStat(25);
                break;
            case CharClassType.Wizard:
                hp = new CharcterStat(150);
                mp = new CharcterStat(450);
                statSTR = new CharcterStat(5);
                statVIT = new CharcterStat(10);
                statINT = new CharcterStat(25);
                StatAGI = new CharcterStat(10);
                break;
            case CharClassType.Warrior:
                hp = new CharcterStat(450);
                mp = new CharcterStat(70);
                statSTR = new CharcterStat(25);
                statVIT = new CharcterStat(20);
                statINT = new CharcterStat(5);
                StatAGI = new CharcterStat(10);
                break;
            case CharClassType.Rogue:
                hp = new CharcterStat(250);
                mp = new CharcterStat(125);
                statSTR = new CharcterStat(10);
                statVIT = new CharcterStat(10);
                statINT = new CharcterStat(10);
                StatAGI = new CharcterStat(25);
                break;
            default:
                break;
        }
    }

    public virtual Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        // result["CID"] = CID;
        result["name"] = name;
        result["classType"] = classType;
        result["level"] = level;
        result["exp"] = exp;
        result["gold"] = gold;
        result["inventory"] = inventory;
        result["equipment"] = equipment;
        result["hp"] = hp.ToDictionary();
        result["mp"] = mp.ToDictionary();
        result["statSTR"] = statSTR.ToDictionary();
        result["statVIT"] = statVIT.ToDictionary();
        result["statINT"] = statINT.ToDictionary();
        result["StatAGI"] = StatAGI.ToDictionary();

        return result;
    }

}
