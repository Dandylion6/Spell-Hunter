using System;
using UnityEngine;

public class CharacterStats
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    
    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }
    
    public int PhysicalResistance { get; private set; }
    public int FireResistance { get; private set; }
    public int ColdResistance { get; private set; }
    public int ShockResistance { get; private set; }
    public int PoisonResistance { get; private set; }
    
    public int Speed { get; private set; }

    public CharacterStats(CharacterDefinition definition)
    {
        MaxHealth = definition.MaxHealth;
        CurrentHealth = MaxHealth;
        
        MaxMana = definition.MaxMana;
        CurrentMana = MaxMana;
        
        PhysicalResistance = definition.PhysicalResistance;
        FireResistance = definition.FireResistance;
        ColdResistance = definition.ColdResistance;
        ShockResistance = definition.ShockResistance;
        PoisonResistance = definition.PoisonResistance;
        
        Speed = definition.Speed;
    }

    public void TakeDamage(int damage) => CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
    public void Heal(int amount) => CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    
    public void ConsumeMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana - amount, 0, MaxMana);
    public void RegenerateMana(int amount) => CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, MaxMana);
    
    // TODO: Implement a robust stat class.
    public void AddPhysicalResistance(int amount) => PhysicalResistance += amount;
    public void AddFireResistance(int amount) => FireResistance += amount;
    public void AddColdResistance(int amount) => ColdResistance += amount;
    public void AddShockResistance(int amount) => ShockResistance += amount;
    public void AddPoisonResistance(int amount) => PoisonResistance += amount;
    public void AddSpeed(int amount) => Speed += amount;
    
    public void RemovePhysicalResistance(int amount) => CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    public void RemoveFireResistance(int amount) => CurrentMana = Mathf.Max(CurrentMana - amount, 0);
    public void RemoveColdResistance(int amount) => CurrentMana = Mathf.Max(CurrentMana - amount, 0);
    public void RemoveShockResistance(int amount) => CurrentMana = Mathf.Max(CurrentMana - amount, 0);
    public void RemovePoisonResistance(int amount) => CurrentMana = Mathf.Max(CurrentMana - amount, 0);
    public void RemoveSpeed(int amount) => CurrentMana = Mathf.Max(CurrentMana - amount, 0);
}