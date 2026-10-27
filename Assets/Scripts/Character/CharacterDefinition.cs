using UnityEngine;

[CreateAssetMenu(fileName = "New Character Definition", menuName = "Objects/Character Definition")]
public class CharacterDefinition : ScriptableObject
{
    [Header("Base Character Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxMana = 40;
    [SerializeField] private int physicalResistance = 10;
    [SerializeField] private int fireResistance = 10;
    [SerializeField] private int coldResistance = 10;
    [SerializeField] private int shockResistance = 10;
    [SerializeField] private int poisonResistance = 10;
    [SerializeField] private int speed = 20;
    
    public int MaxHealth => maxHealth;
    public int MaxMana => maxMana;
    public int PhysicalResistance => physicalResistance;
    public int FireResistance => fireResistance;
    public int ColdResistance => coldResistance;
    public int ShockResistance => shockResistance;
    public int PoisonResistance => poisonResistance;
    public int Speed => speed;
}
