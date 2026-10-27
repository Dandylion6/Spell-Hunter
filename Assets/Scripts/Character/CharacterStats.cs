using System;
using UnityEngine;

public class CharacterStats
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    
    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }
    
    public Stat PhysicalResistance { get; private set; }
    public Stat FireResistance { get; private set; }
    public Stat ColdResistance { get; private set; }
    public Stat ShockResistance { get; private set; }
    public Stat PoisonResistance { get; private set; }
    
    public Stat Speed { get; private set; }

    public CharacterStats(CharacterDefinition definition)
    {
        MaxHealth = definition.MaxHealth;
        CurrentHealth = MaxHealth;
        
        MaxMana = definition.MaxMana;
        CurrentMana = MaxMana;
        
        PhysicalResistance = new Stat(definition.PhysicalResistance);
        FireResistance = new Stat(definition.FireResistance);
        ColdResistance = new Stat(definition.ColdResistance);
        ShockResistance = new Stat(definition.ShockResistance);
        PoisonResistance = new Stat(definition.PoisonResistance);
        
        Speed = new Stat(definition.Speed);
    }

    public void TakeDamage(int damage) => CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
    public void Heal(int amount) => CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    
    public void ConsumeMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana - amount, 0, MaxMana);
    public void RegenerateMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, MaxMana);
}