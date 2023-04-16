using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Chibis and Dungeons/Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    private string uniqueID;
    public string ItemName;
    [Range(1, 999)]
    public int MaxStack = 1;
    public Sprite Icon;
    public int EquipableLV;
    public CharClassType EquipableClass;

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {
    }
}

