
using System;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public string name;

    public Player()
    {
    }

    public Player(string _name)
    {
        name = _name;
    }

    public Player(Dictionary<string, Object> _dictionary)
    {
        name = _dictionary["name"].ToString();
    }

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["name"] = name;

        return result;
    }

}
