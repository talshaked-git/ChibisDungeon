
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
    public string CID { get; set; }
    public string name { get; set; }
    public CharClassType classType { get; set; }
    public int level { get; set; }
    public long exp { get; set; }
    public int gold { get; set; }
    public CharcterStat HP { get; set; }
    public CharcterStat MP { get; set; }
    public CharcterStat STR { get; set; }
    public CharcterStat VIT { get; set; }
    public CharcterStat INT { get; set; }
    public CharcterStat AGI { get; set; }
    

    public Player(string _name,string _CID ,CharClassType _class)
    {
        CID = _CID;
        name = _name;
        classType = _class;
        level = 1;
        exp = 0;
        gold = 100;
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
        exp = Convert.ToInt64(_dictionary["exp"]);
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

    private void InitStatesByClassType(){
        switch (classType)
        {
            case CharClassType.Archer:        
                HP = new CharcterStat(250);
                MP = new CharcterStat(150);
                STR = new CharcterStat(15);
                VIT = new CharcterStat(10);
                INT = new CharcterStat(5);
                AGI = new CharcterStat(25);
                break;
            case CharClassType.Wizard:
                HP = new CharcterStat(150);
                MP = new CharcterStat(450);
                STR = new CharcterStat(5);
                VIT = new CharcterStat(10);
                INT = new CharcterStat(25);
                AGI = new CharcterStat(10);
                break;
            case CharClassType.Warrior:
                HP = new CharcterStat(450);
                MP = new CharcterStat(70);
                STR = new CharcterStat(25);
                VIT = new CharcterStat(20);
                INT = new CharcterStat(5);
                AGI = new CharcterStat(10);
                break;
            case CharClassType.Rogue:
                HP = new CharcterStat(250);
                MP = new CharcterStat(125);
                STR = new CharcterStat(10);
                VIT = new CharcterStat(10);
                INT = new CharcterStat(10);
                AGI = new CharcterStat(25);
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
        result["exp"] = exp;
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
        return "Player [name=" + name + ", classType=" + classType + ", level=" + level + ", exp=" + exp + ", gold=" + gold + ", HP=" + HP + ", MP=" + MP + ", STR=" + STR + ", VIT=" + VIT + ", INT=" + INT + ", AGI=" + AGI + "]";
    }


}
