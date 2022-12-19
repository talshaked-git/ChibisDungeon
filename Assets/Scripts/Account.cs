using System;
using System.Collections.Generic;

[System.Serializable]
public class Account
{
    public string UID { get; set; }

    public Player[] players { get; set; }

    public Account()
    {
        players = new Player[4];
    }

    public Account(string _UID)
    {
        UID = _UID;
        players = new Player[4];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player("Player " + (i + 1));
        }
    }

    public Account(Dictionary<string, Object> _dictionary)
    {
        UID = _dictionary["UID"].ToString();

        players = new Player[4];
        for (int i = 0; i < players.Length; i++)
        {
            if (_dictionary.ContainsKey("player" + (i + 1)))
            {
                players[i] = new Player((Dictionary<string, Object>)_dictionary["player" + (i + 1)]);
            }
        }
    }

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["UID"] = UID;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
                continue;
            result["player" + (i + 1)] = players[i].ToDictionary();
        }

        return result;
    }



}
