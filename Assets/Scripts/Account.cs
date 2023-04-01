using System;
using System.Collections.Generic;

[System.Serializable]
public class Account
{
    public string UID { get; set; }
    public List<Player> players;

    public Account()
    {
        players = new List<Player>();
    }

    public Account(string _UID) : this()
    {
        UID = _UID;
    }

    public Account(Dictionary<string, Object> _dictionary)
    {
        UID = _dictionary["UID"].ToString();

        players = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            if (_dictionary.ContainsKey("player" + (i + 1)))
            {
                players.Add(new Player((Dictionary<string, Object>)_dictionary["player" + (i + 1)]));
            }
        }
    }

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["UID"] = UID;
        int i = 0;
        foreach (Player player in players)
        {
            if (players[i] == null)
            {
                i++;
                continue;
            }
            result["player" + (i + 1)] = players[i].ToDictionary();
            i++;
        }

        return result;
    }

    public void AddPlayer(Player _player)
    {
        players.Add(_player);
    }



}
