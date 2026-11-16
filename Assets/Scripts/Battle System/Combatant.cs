using System.Collections.Generic;
using System.Linq;

public enum CombatantType
{
    Ally,
    Opponent
}

public class Combatant
{
    public string DisplayName { get; private set; }
    public CharacterStats Stats { get; private set; }
    public TurnClock Clock { get; private set; }
    public IActionProvider Brain { get; private set; }
    public CombatantType CombatantType { get; private set; }
    public List<StatusEffect> StatusEffects { get; }
    
    public bool IsDead => Stats.CurrentHealth <= 0;
    
    public IReadOnlyList<Ability> Abilities { get; }


    public Combatant(CharacterStats stats, IActionProvider brain, CombatantType combatantType,
        IReadOnlyList<AbilityBlueprint> abilityBlueprints, AbilityFactory abilityFactory)
    {
        DisplayName = stats.BaseName;
        Stats = stats;
        Clock = new TurnClock();
        Brain = brain;
        CombatantType = combatantType;
        Abilities = BuildAbilities(abilityBlueprints, abilityFactory);
        StatusEffects = new List<StatusEffect>();
    }

    public void SetName(string displayName) => DisplayName = displayName;

    private List<Ability> BuildAbilities(IReadOnlyList<AbilityBlueprint> abilityBlueprints,
        AbilityFactory abilityFactory)
    {
        List<Ability> abilities = new(abilityBlueprints.Count);
        abilities.AddRange(abilityBlueprints.Select(blueprint => abilityFactory.CreateAbility(blueprint, this)));
        return abilities;
    }

    public bool AddStatusEffect(StatusEffect statusEffect, List<BattleEvent> events)
    {
        if (statusEffect == null) return false;
        if (!statusEffect.CanBeApplied()) return false;

        StatusEffect existing = StatusEffects.FirstOrDefault(effect => effect.GetType() == statusEffect.GetType());
        if (existing != null)
        {
            existing.OnRefresh(statusEffect, events);
            return true;
        }

        StatusEffects.Add(statusEffect);
        statusEffect.OnApply(events);
        return true;
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        if (!StatusEffects.Contains(statusEffect)) return;

        StatusEffects.Remove(statusEffect);
        statusEffect.OnRemove();
    }
}