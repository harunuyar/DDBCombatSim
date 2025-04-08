namespace DDBCombatSim.GameSystem.Combatant;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Battlefield;
using DDBCombatSim.GameSystem.Effect;
using DDBCombatSim.GameSystem.Stats;
using DDBCombatSim.GameSystem.Utils;

public abstract class CombatantBase : ICombatant, IDndObject
{
    public CombatantBase(string id, string name, ECreatureType creatureType, IntStat armorClass, IntStat maxHp, int currentHp, int tempHp, IntStat[] abilityScores, IntStat[] savingThrows)
    {
        Id = id;
        Name = name;
        CreatureType = creatureType;
        Position = new();
        ArmorClass = armorClass;
        HitPoints = new ConsumableStat(maxHp)
        {
            Value = currentHp
        };
        TempHp = new ConsumableStat(new IntStat("Base", 0))
        {
            Value = tempHp
        };
        AbilityScores = abilityScores;
        SavingThrowModifiers = savingThrows;
        BonusActions = new ConsumableStat(new IntStat("Bonus Actions", 1));
        Reactions = new ConsumableStat(new IntStat("Reactions", 1));
        Actions = new ConsumableStat(new IntStat("Actions", 1));
        MagicActions = new ConsumableStat(new IntStat("Magic Actions", 1));
        CombatActions = [];
        ActiveEffects = [];
        CausedEffects = [];
        DeathStatus = new();
        Width = 1;
        Height = 1;
        OccupiedTiles = [];
        Speed = new ConsumableStat(new IntStat("Speed", 30));
        Resistances = EDamageType.None;
        Immunities = EDamageType.None;
        Vulnurabilities = EDamageType.None;
        SpellSlots = new ConsumableStat[10];
        for (int i = 0; i < SpellSlots.Length; i++)
        {
            SpellSlots[i] = new ConsumableStat(new IntStat($"Default", 0));
        }
    }

    public string Id { get; }
    public string Name { get; }
    public ECreatureType CreatureType { get; }
    public ConsumableStat HitPoints { get; }
    public ConsumableStat TempHp { get; }
    public IntStat ArmorClass { get; }
    public IntStat[] AbilityScores { get; }
    public IntStat[] SavingThrowModifiers { get; }
    public IntStat[] AbilityModifiers => [.. AbilityScores.Select(score => new IntStat(score.Name, (score.Value / 2 - 5)))];
    public List<IAction> CombatActions { get; }
    public Position Position { get; }
    public virtual ECoverType CoverType => ECoverType.Full;
    public ConsumableStat BonusActions { get; set; }
    public ConsumableStat Reactions { get; set; }
    public ConsumableStat Actions { get; set; }
    public ConsumableStat MagicActions { get; set; }
    public DeathStatus DeathStatus { get; }
    public List<EffectInstance> ActiveEffects { get; }
    public List<EffectInstance> CausedEffects { get; }
    public int Height { get; }
    public int Width { get; }
    public HashSet<Tile> OccupiedTiles { get; }
    public ConsumableStat Speed { get; }
    public EDamageType Resistances { get; set; }
    public EDamageType Immunities { get; set; }
    public EDamageType Vulnurabilities { get; set; }

    public ConsumableStat[] SpellSlots { get; }

    public abstract IntStat GetAbilityModifier(EAbility ability);
}
