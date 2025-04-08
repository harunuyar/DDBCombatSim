namespace DDBCombatSim.GameSystem.Stats;

public class IntStat : IStat<int>, ICloneable
{
    public IntStat(string name, int baseValue)
    {
        Name = name;
        BaseValue = baseValue;
        Modifiers = [];
    }

    public string Name { get; set; }

    public int BaseValue { get; set; }

    public Modifier<int>? OverridingValue { get; set; }

    public List<Modifier<int>> Modifiers { get; }

    public int Value => OverridingValue?.Value ?? (BaseValue + Modifiers.Sum(m => m.Value));

    public void AddOtherAsModifier(IStat<int> other)
    {
        if (other.BaseValue != 0)
        {
            Modifiers.Add(new Modifier<int>(other, other.Name, other.BaseValue));
        }

        if (other is IntStat intStat)
        {
            Modifiers.AddRange(intStat.Modifiers);
        }
    }

    public IntStat Clone()
    {
        var clone = new IntStat(Name, BaseValue);
        clone.OverridingValue = OverridingValue?.Clone();
        clone.Modifiers.AddRange(Modifiers.Select(m => m.Clone()));
        return clone;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
