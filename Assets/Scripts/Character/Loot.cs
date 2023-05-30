using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Chibis and Dungeons/Loot/Loot Item")]
public class Loot : ScriptableObject
{
    public Item item;
    public int amount;
    [Range(1,100)]
    public int chance;
    public Loot(Item item, int amount, int chance)
    {
        this.item = item;
        this.amount = amount;
        this.chance = chance;
    }

    public void SetLootChance(int chance)
    {
        this.chance = chance;
    }
}
