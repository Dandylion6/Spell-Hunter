using System.Collections.Generic;
using System.Linq;

public class Burn : StatusEffect
{
    public class Stack
    {
        public int Damage { get; }
        public int PhysicalResistMelt { get; }
        public int DurationRemaining { get; private set; }

        public Stack(int damage, int physicalResistMelt, int duration)
        {
            Damage = damage;
            PhysicalResistMelt = physicalResistMelt;
            DurationRemaining = duration;
        }

        public void Tick() => --DurationRemaining;
    }

    public override bool IsExpired => Stacks.Count == 0;
    public List<Stack> Stacks { get; }

    public Stat MaxStacks { get; }
    public Stat Damage { get; private set; }
    public Stat PhysicalResistMelt { get; private set; }
    public Stat MaxPhysicalResistMelt { get; private set; }
    public Stat StackDuration { get; private set; }
    public Stat StacksOnProc { get; private set; }
    public int CurrentResistMelt { get; private set; }

    public Burn(Combatant target, string displayName, int damage, int physicalResistMelt, int maxPhysicalResistMelt,
        int stackDuration, int maxStacks, int stacksOnProc) : base(target, displayName)
    {
        Stacks = new List<Stack>();
        Damage = new Stat(damage);
        PhysicalResistMelt = new Stat(physicalResistMelt);
        MaxPhysicalResistMelt = new Stat(maxPhysicalResistMelt);
        StackDuration = new Stat(stackDuration);
        MaxStacks = new Stat(maxStacks);
        StacksOnProc = new Stat(stacksOnProc);
        CurrentResistMelt = 0;
    }

    public override void OnApply(List<BattleEvent> events) => ProcBurn();

    public override void OnRefresh(StatusEffect incomingEffect, List<BattleEvent> events)
    {
        if (incomingEffect is not Burn newBurn)
            throw new System.ArgumentException("incoming effect is not of type Burn", nameof(incomingEffect));

        // Shallow copy all the new stats.
        Damage = newBurn.Damage;
        PhysicalResistMelt = newBurn.PhysicalResistMelt;
        MaxPhysicalResistMelt = newBurn.MaxPhysicalResistMelt;
        StackDuration = newBurn.StackDuration;
        StacksOnProc = newBurn.StacksOnProc;

        ProcBurn();
    }

    public override void OnBeforeAction(Battle battle, Turn currentTurn, List<BattleEvent> events)
    {
        if (IsExpired) return;
        List<Stack> tickBuffer = new(Stacks);

        int totalDamage = 0;
        foreach (Stack stack in tickBuffer.Where(stack => Stacks.Contains(stack)))
        {
            totalDamage += stack.Damage;

            StatModifier resistMelt = new(this, StatModifierType.Flat, -stack.PhysicalResistMelt);
            CurrentResistMelt += -stack.PhysicalResistMelt;
            Target.Stats.PhysicalResistance.AssignModifier(resistMelt);

            stack.Tick();
            if (stack.DurationRemaining <= 0) RemoveStack(stack);
        }

        int oldHealth = Target.Stats.CurrentHealth;
        Damage damage = new(DamageType.Fire, totalDamage, DamageTags.Status | DamageTags.Elemental);
        int damageTaken = Target.Stats.TakeDamage(damage);

        events.Add(new DamageFromStatusEvent(damage.Tags, damage.Type, damageTaken, oldHealth,
            Target.Stats.CurrentHealth, this, Target));
    }

    public override string GetTooltip()
    {
        throw new System.NotImplementedException();
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
            RemoveStack(Stacks.OrderBy(stack => stack.DurationRemaining).FirstOrDefault());
        }

        if (MaxStacks.Value <= 0) return;

        Stack newStack = new(Damage.Value, PhysicalResistMelt.Value, StackDuration.Value);
        Stacks.Add(newStack);
    }

    private void RemoveStack(Stack stack)
    {
        if (!Stacks.Contains(stack)) return;
        Stacks.Remove(stack);
    }
}