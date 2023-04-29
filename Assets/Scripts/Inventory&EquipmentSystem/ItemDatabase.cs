using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Chibis and Dungeons/Item/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private Item[] items;

    public Item GetItemRefference(string id)
    {
        foreach (Item item in items)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public Item GetItemCopy(string id)
    {
        try
        {
            Item item = GetItemRefference(id);
            if (item == null) return null;
            return item.GetCopy();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        LoadItems();
    }

    private void OnEnable()
    {
        EditorApplication.projectChanged += LoadItems;
    }

    private void OnDisable()
    {
        EditorApplication.projectChanged -= LoadItems;
    }

    private void LoadItems()
    {
        items = FindAssetsByType<Item>("Assets/Resources/Items");
    }

    public static T[] FindAssetsByType<T>(params string[] folders) where T : UnityEngine.Object
    {
        string type = typeof(T).ToString().Replace("UnityEngine.", "");

        string[] guids;
        if (folders == null || folders.Length == 0)
        {
            guids = AssetDatabase.FindAssets("t:" + type);
        }
        else
        {
            guids = AssetDatabase.FindAssets("t:" + type, folders);
        }

        T[] assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
        return assets;
    }
    #endif
}
