using UnityEngine;

[CreateAssetMenu(fileName = "New Base Stats", menuName = "Objects/AbilityBaseStats")]
public class AbilityBaseStats : ScriptableObject
{
    [SerializeField] private ShotBaseStats shotBaseStats;
    [SerializeField] private HexBaseStats hexBaseStats;
    [SerializeField] private BoonBaseStats boonBaseStats;
    
    public ShotBaseStats Shot => shotBaseStats;
    public HexBaseStats Hex => hexBaseStats;
    public BoonBaseStats Boon => boonBaseStats;
}

[System.Serializable]
public class ShotBaseStats
{
    [SerializeField] private int manaCost = 1;
    [SerializeField] private int physicalDamage = 10;
    [SerializeField] private int onHitProcChance = 20;
    
    public int ManaCost => manaCost;
    public int PhysicalDamage => physicalDamage;
    public int OnHitProcChance => onHitProcChance;
}

[System.Serializable]
public class HexBaseStats
{
    [SerializeField] private int manaCost = 8;
    [SerializeField] private int onHitProcDamagePercentageIncrease = 100;
    
    public int ManaCost => manaCost;
    public int OnHitProcDamagePercentageIncrease => onHitProcDamagePercentageIncrease;
}

[System.Serializable]
public class BoonBaseStats
{
    [SerializeField] private int manaCost = 6;
    
    public int ManaCost => manaCost;
}