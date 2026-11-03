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
    }

    public void TakeDamage(int damage) => CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth.Value);
    public void Heal(int amount) => CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth.Value);
    
    public void ConsumeMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana - amount, 0, MaxMana.Value);
    public void RegenerateMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, MaxMana.Value);
}