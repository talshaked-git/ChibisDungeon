using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Firebase.Firestore;

[CreateAssetMenu(fileName = "Item", menuName = "Chibis and Dungeons/Item/Item")]

public class Item : ScriptableObject
{

    [SerializeField] private string _id;

    public string ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public string uniqueID { get; set; }


    [SerializeField] private string _itemName;
    public string ItemName { 
        get { return _itemName; } 
        set { _itemName = value; } 
    }

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
}

