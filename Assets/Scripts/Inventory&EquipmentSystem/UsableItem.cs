using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Chibis and Dungeons/Item/Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable;
    public List<UsableItemEffect> Effects;

    public virtual void Use(Player player)
    {
        foreach (UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, player);
        }
    }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        foreach (UsableItemEffect effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        }
        return sb.ToString();
    }

    public override Dictionary<string, System.Object> ToDictionary()
    {
        return base.ToDictionary();
    }

    public override void FromDictionary(Dictionary<string, System.Object> dict)
    {
        base.FromDictionary(dict);
    }
}
