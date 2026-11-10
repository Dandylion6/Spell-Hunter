using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BurnBaseStats
{
    [SerializeField] [Min(0)] private int stackFireDamage = 3;
    [SerializeField] [Min(0)] private int stackPhysicalResistMelt = 6;
    [SerializeField] [Min(0)] private int maxPhysicalResistMelt = 30;
    [SerializeField] [Min(1)] private int stackTurns = 4;
    [SerializeField] [Min(1)] private int maxStacks = 2;
    [SerializeField] [Min(1)] private int stacksOnProc = 1;

    public int StackFireDamage => stackFireDamage;
    public int StackPhysicalResistMelt => stackPhysicalResistMelt;
    public int MaxPhysicalResistMelt => maxPhysicalResistMelt;
    public int StackTurns => stackTurns;
    public int MaxStacks => maxStacks;
    public int StacksOnProc => stacksOnProc;
}

public class Burn : StatusEffect
{
    public class Stack
    {
        public int FireDamage { get; }
        public int PhysicalResistMelt { get; }
        public int TurnsRemaining { get; private set; }

        public Stack(int fireDamage, int physicalResistMelt, int turns)
        {
            FireDamage = fireDamage;
            PhysicalResistMelt = physicalResistMelt;
            TurnsRemaining = turns;
        }

        public void Tick() => --TurnsRemaining;
    }

    public override bool IsExpired => Stacks.Count == 0;
    public List<Stack> Stacks { get; }

    public Stat MaxStacks { get; }
    public Stat StackFireDamage { get; private set; }
    public Stat StackPhysicalResistMelt { get; private set; }
    public Stat MaxPhysicalResistMelt { get; private set; }
    public Stat StackTurns { get; private set; }
    public Stat StacksOnProc { get; private set; }
    public int CurrentResistMelt { get; private set; }

    public Burn(Combatant target, BurnBaseStats burnBaseStats) : base(target, "Burn")
    {
        Stacks = new List<Stack>();
        StackFireDamage = new Stat(burnBaseStats.StackFireDamage);
        StackPhysicalResistMelt = new Stat(burnBaseStats.StackPhysicalResistMelt);
        MaxPhysicalResistMelt = new Stat(burnBaseStats.MaxPhysicalResistMelt);
        StackTurns = new Stat(burnBaseStats.StackTurns);
        MaxStacks = new Stat(burnBaseStats.MaxStacks);
        StacksOnProc = new Stat(burnBaseStats.StacksOnProc);
        CurrentResistMelt = 0;
    }

    public override void OnApply() => ProcBurn();

    public override void OnRefresh(StatusEffect incomingEffect)
    {
        if (incomingEffect is not Burn newBurn)
            throw new System.ArgumentException("incoming effect is not of type Burn", nameof(incomingEffect));

        // Shallow copy all the new stats.
        StackFireDamage = newBurn.StackFireDamage;
        StackPhysicalResistMelt = newBurn.StackPhysicalResistMelt;
        MaxPhysicalResistMelt = newBurn.MaxPhysicalResistMelt;
        StackTurns = newBurn.StackTurns;
        StacksOnProc = newBurn.StacksOnProc;

        ProcBurn();
    }

    public override void OnBeforeAction(Battle battle, List<BattleEvent> events)
    {
        if (IsExpired) return;
        List<Stack> tickBuffer = new(Stacks);

        int totalDamage = 0;
        foreach (Stack stack in tickBuffer.Where(stack => Stacks.Contains(stack)))
        {
            totalDamage += stack.FireDamage;

            StatModifier resistMelt = new(this, StatModifierType.Flat, -stack.PhysicalResistMelt);
            CurrentResistMelt += -stack.PhysicalResistMelt;
            Target.Stats.PhysicalResistance.AssignModifier(resistMelt);

            stack.Tick();
            if (stack.TurnsRemaining <= 0) RemoveStack(stack);
        }
        
        int oldHealth = Target.Stats.CurrentHealth;
        Damage damage = new(DamageType.Fire, totalDamage, DamageTags.Status | DamageTags.Elemental);
        int damageTaken = Target.Stats.TakeDamage(damage);
        
        events.Add(new DamageFromStatusEvent(damage.Tags, damage.Type, damageTaken, oldHealth,
            Target.Stats.CurrentHealth, this, Target));
    }

    public override void OnRemove()
    {
        Target.Stats.PhysicalResistance.RemoveModifiersFromSource(this);
        while (Stacks.Count > 0)
        {
            RemoveStack(Stacks.FirstOrDefault()); // Purges all remaining stacks before burn removal.
        }
    }

    private void ProcBurn()
    {
        for (int i = 0; i < StacksOnProc.Value; ++i) AddStack();
    }

    private void AddStack()
    {
        while (Stacks.Count > 0 && Stacks.Count >= MaxStacks.Value)
        {
            RemoveStack(Stacks.OrderBy(stack => stack.TurnsRemaining).FirstOrDefault());
        }

        if (MaxStacks.Value <= 0) return;

        Stack newStack = new(StackFireDamage.Value, StackPhysicalResistMelt.Value, StackTurns.Value);
        Stacks.Add(newStack);
    }

    private void RemoveStack(Stack stack)
    {
        if (!Stacks.Contains(stack)) return;
        Stacks.Remove(stack);
    }
}