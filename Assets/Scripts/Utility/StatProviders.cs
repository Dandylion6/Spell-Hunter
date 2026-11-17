using System.Collections.Generic;

public interface IStatProvider
{
    public IEnumerable<Stat> GetStats();
}

public interface IDamage : IStatProvider
{
}

public interface IHeal : IStatProvider
{
}

public interface IDuration : IStatProvider
{
}