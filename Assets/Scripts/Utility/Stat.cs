using System;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier
{
    public enum ModifierType
    {
        /// <summary>
        ///     Adds to the base value as a flat value.
        /// </summary>
        Flat,
        /// <summary>
        ///     Adds based on the flat value as percentage.
        /// </summary>
        Percentage,
        /// <summary>
        ///     Multiplies the value by an amount.
        /// </summary>
        Multiplier
    }
    
    public object Source { get; private set; }
    public ModifierType Type { get; private set; }
    public float Value { get; private set; }

    public StatModifier(object source, ModifierType type, float value)
    {
        Source = source;
        Type = type;
        Value = value;
    }
}

public class Stat
{
    /// <summary>
    ///     The value before modifiers are used.
    /// </summary>
    public int BaseValue { get; private set; }

    /// <summary>
    ///     The value calculated with all the modifiers.
    /// </summary>
    public int Value
    {
        get
        {
            if (isDirty) UpdateValue();
            return cachedValue;
        }
    }
    
    public IReadOnlyList<StatModifier> Modifiers => modifiers;
    private readonly List<StatModifier> modifiers = new();
    private int cachedValue;
    private bool isDirty = true;
    
    public Stat(int baseValue) => BaseValue = baseValue;

    public void SetBaseValue(int baseValue)
    {
        BaseValue = baseValue;
        MarkDirty();
    }
    
    /// <summary>
    ///     Assigns the modifier to the stat.
    /// </summary>
    /// <param name="modifier">
    ///     is the modifier to assign to the stat. The same instance can't be used for the same stat.
    /// </param>
    public void AssignModifier(StatModifier modifier)
    {
        if (modifier == null) return;
        if (modifiers.Contains(modifier)) return;
        
        modifiers.Add(modifier);
        MarkDirty();
    }
    
    /// <summary>
    ///     Removes all modifiers belonging to a source (like a skill).
    /// </summary>
    /// <param name="source">
    ///     is the origin of the modifiers to remove.
    /// </param>
    public void RemoveModifiersFromSource(object source)
    {
        if (source == null) return;
        modifiers.RemoveAll(modifier => modifier.Source == source);
        MarkDirty();
    }

    private void UpdateValue()
    {
        float flat = 0.0f, percentage = 0.0f, multiplier = 1.0f;

        foreach (StatModifier modifier in modifiers)
        {
            switch (modifier.Type)
            {
                case StatModifier.ModifierType.Flat: flat += modifier.Value; break;
                case StatModifier.ModifierType.Percentage: percentage += modifier.Value; break;
                case StatModifier.ModifierType.Multiplier: multiplier *= 1.0f + modifier.Value; break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        cachedValue = Mathf.RoundToInt((BaseValue + flat) * (1.0f + percentage / 100.0f) * multiplier);
        isDirty = false;
    }

    private void MarkDirty() => isDirty = true;
}