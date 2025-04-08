namespace DDBCombatSim.GameSystem.Stats;

public class ConsumableStat
{
    public ConsumableStat(IntStat stat)
    {
        BaseStat = stat;
        Value = stat.Value;
    }

    public IntStat BaseStat { get; }

    public int MaxValue => BaseStat.Value;

    public int Value { get; set; }

    public void Reset()
    {
        Value = BaseStat.Value;
    }

    public void Use(int amount = 1)
    {
        Value -= amount;
    }

    public void Restore(int amount = 1)
    {
        Value = Math.Min(Value + amount, BaseStat.Value);
    }

    public bool CanUse(int amount = 1)
    {
        return Value >= amount;
    }
}
