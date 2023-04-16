using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class CharcterStat
{
    public float BaseValue;
    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    public delegate void OnStatChanged();
    public event OnStatChanged StatChanged;

    public virtual float Value
    {
        get
        {
            if (isDirty || lastBaseValue != BaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }
    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;

    public CharcterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharcterStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public CharcterStat(Dictionary<string, Object> _dictionary)
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();

        BaseValue = float.Parse(_dictionary["BaseValue"].ToString());
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
        StatChanged?.Invoke();
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0; //if (a.Order == b.Order)
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            StatChanged?.Invoke();
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        if (didRemove)
            StatChanged?.Invoke();

        return didRemove;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd)
            {
                sumPercentAdd += mod.Value;

                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }

    //Discuss to change save of statModifier or create from equiped items
    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["BaseValue"] = BaseValue;
        return result;
    }
}
