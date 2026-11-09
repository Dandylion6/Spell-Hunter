using UnityEngine;

[CreateAssetMenu(fileName = "New Base Stats", menuName = "Objects/AbilityBaseStats")]
public class AbilityBaseStatsConfig : ScriptableObject
{
    [SerializeField] private ShotBaseStats shotBaseStats;
    [SerializeField] private HexBaseStats hexBaseStats;
    [SerializeField] private BoonBaseStats boonBaseStats;
    
    public ShotBaseStats Shot => shotBaseStats;
    public HexBaseStats Hex => hexBaseStats;
    public BoonBaseStats Boon => boonBaseStats;
}

[System.Serializable]
public abstract class AbilityBaseStats
{
    [Header("Base Stats")]
    [SerializeField] private int manaCost = 1;
    [SerializeField] [Range(20, 200)] private int turnCost = 100;
    [SerializeField] [Range(1, 10)] private int augmentSlots = 4;
    
    public int ManaCost => manaCost;
    public int TurnCost => turnCost;
    public int AugmentSlots => augmentSlots;
}

[System.Serializable]
public class ShotBaseStats : AbilityBaseStats
{
    [Header("Shot Stats")]
    [SerializeField] private int physicalDamage = 10;
    [SerializeField] private int accuracy = 95;
    [SerializeField] private int statusProcChance = 20;
    [SerializeField] private int onHitBleedChance = 20;
    
    public int PhysicalDamage => physicalDamage;
    public int Accuracy => accuracy;
    public int StatusProcChance => statusProcChance;
    public int OnHitBleedChance => onHitBleedChance;
}

[System.Serializable]
public class HexBaseStats : AbilityBaseStats
{
    [Header("Hex Stats")]
    [SerializeField] private int durationPercentageIncrease = 50;
    
    public int DurationPercentageIncrease => durationPercentageIncrease;
}

[System.Serializable]
public class BoonBaseStats : AbilityBaseStats
{
}