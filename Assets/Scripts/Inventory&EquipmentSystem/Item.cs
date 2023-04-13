using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Chibis and Dungeons/Item", order = 0)]
public class Item : ScriptableObject {
    public int ItemID;
    public string ItemName;
    public Sprite Icon;
    public int EquipableLV;
    public CharClassType EquipableClass;
}

