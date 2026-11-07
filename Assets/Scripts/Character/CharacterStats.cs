using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats
{
    public string BaseName { get; }
    public int CurrentHealth { get; private set; }
    public Stat MaxHealth { get; }
    
    public int CurrentMana { get; private set; }
    public Stat MaxMana { get; }
    
    public Stat PhysicalResistance { get; private set; }
    public Stat FireResistance { get; private set; }
    public Stat ColdResistance { get; private set; }
    public Stat ShockResistance { get; private set; }
    public Stat PoisonResistance { get; private set; }
    
    public Stat Speed { get; private set; }
    
    public Stat IncomingDamagePercentage { get; private set; }

    public CharacterStats(CharacterDefinition definition)
    {
        BaseName = definition.DisplayName;
        MaxHealth = new  Stat(definition.MaxHealth);
        CurrentHealth = MaxHealth.Value;
        
        MaxMana = new Stat(definition.MaxMana);
        CurrentMana = MaxMana.Value;
        
        PhysicalResistance = new Stat(definition.PhysicalResistance);
        FireResistance = new Stat(definition.FireResistance);
        ColdResistance = new Stat(definition.ColdResistance);
        ShockResistance = new Stat(definition.ShockResistance);
        PoisonResistance = new Stat(definition.PoisonResistance);
        
        Speed = new Stat(definition.Speed);
        IncomingDamagePercentage = new Stat(100);
    }

    public void Heal(int amount) => CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth.Value);

    public Stat GetResistanceTo(DamageType type)
    {
        return type switch
        {
            DamageType.Physical => PhysicalResistance,
            DamageType.Fire => FireResistance,
            DamageType.Cold => ColdResistance,
            DamageType.Shock => ShockResistance,
            DamageType.Toxin => PoisonResistance,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static float GetDamageMultiplier(float resistance)
    {
        return resistance >= 0.0f ? 100.0f / (100.0f + resistance) : 2.0f - 100.0f / (100.0f - resistance);
    }
    
    public int TakeDamage(Damage damage)
    {
        int takenDamage = damage.Amount.Value;
        
        damage.Amount.AssignModifier(new StatModifier(this, StatModifierType.Percentage, IncomingDamagePercentage.Value));
        
        if (!damage.IgnoresResistance)
        {
            float multiplier = GetDamageMultiplier(GetResistanceTo(damage.Type).Value);
            takenDamage = Mathf.RoundToInt(takenDamage * multiplier);
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth - takenDamage, 0, MaxHealth.Value);
        return takenDamage;
    }

    public int ConsumeMana(int amount)
    {
        CurrentMana = Mathf.Clamp(CurrentMana - amount, 0, MaxMana.Value);
        return CurrentMana;
    }

    public int RegenerateMana(int amount)
    {
        CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, MaxMana.Value);
        return CurrentMana;
    }
}