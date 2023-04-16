using System;
using System.Collections.Generic;

/// <summary>
/// StatModType enumeration provides different types of stat modifiers.
/// </summary>
public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

/// <summary>
/// The StatModifier class represents a stat modifier that can be applied to various game stats.
/// </summary>
public class StatModifier
{
    public readonly float Value;
    public readonly StatModType Type;
    public readonly int Order;
    public readonly object Source;

    /// <summary>
    /// Constructs a StatModifier with the specified value, type, order, and source.
    /// </summary>
    /// <param name="value">The float value of the stat modifier.</param>
    /// <param name="type">The StatModType enumeration value representing the type of stat modifier.</param>
    /// <param name="order">The int order in which the stat modifier should be applied.</param>
    /// <param name="source">The object source of the stat modifier.</param>
    public StatModifier(float value, StatModType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    /// <summary>
    /// Constructs a StatModifier with the specified value and type, using the default order and a null source.
    /// </summary>
    /// <param name="value">The float value of the stat modifier.</param>
    /// <param name="type">The StatModType enumeration value representing the type of stat modifier.</param>
    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

    /// <summary>
    /// Constructs a StatModifier with the specified value, type, and order, using a null source.
    /// </summary>
    /// <param name="value">The float value of the stat modifier.</param>
    /// <param name="type">The StatModType enumeration value representing the type of stat modifier.</param>
    /// <param name="order">The int order in which the stat modifier should be applied.</param>
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

    /// <summary>
    /// Constructs a StatModifier with the specified value, type, and source, using the default order.
    /// </summary>
    /// <param name="value">The float value of the stat modifier.</param>
    /// <param name="type">The StatModType enumeration value representing the type of stat modifier.</param>
    /// <param name="source">The object source of the stat modifier.</param>
    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
}
