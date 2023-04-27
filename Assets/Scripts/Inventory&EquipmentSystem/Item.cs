using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "Chibis and Dungeons/Item/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    protected List<string> foldersToSearch = new List<string>
    {
        "Items/Consumables",
        "Items/Equipment/Bows",
        "Items/Equipment/Chest",
        "Items/Equipment/Staves",
        "Items/Equipment/Swords"
    };

    [SerializeField] string id;
    public string ID
    {
        get { return id; }
        set { id = value; }
    }
    protected string uniqueID;
    public string ItemName;
    [Range(1, 999)]
    public int MaxStack = 1;
    public Sprite Icon;
    public int EquipableLV;
    public CharClassType EquipableClass;

    protected static readonly StringBuilder sb = new StringBuilder();

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {
    }

    public virtual string GetItemType()
    {
        return "";
    }

    public virtual string GetDescription()
    {
        return string.Empty;
    }

    public virtual Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["ItemName"] = ItemName;
        if (uniqueID == null)
            uniqueID = Guid.NewGuid().ToString();
        result["UniqueID"] = uniqueID;

        return result;
    }

    public virtual void FromDictionary(Dictionary<string, System.Object> dict)
    {
        uniqueID = (string)dict["UniqueID"];
    }

}

