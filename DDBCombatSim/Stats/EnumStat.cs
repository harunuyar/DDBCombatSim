namespace DDBCombatSim.Stats;

using System;

public class EnumStat<T> : IStat<T>, ICloneable where T : struct, Enum
{
    public EnumStat(string name, T baseValue)
    {
        Name = name;
        BaseValue = baseValue;
        Modifiers = [];
    }

    public string Name { get; set; }

    public T BaseValue { get; set; }

    public Modifier<T>? OverridingValue { get; set; }

    public List<Modifier<T>> Modifiers { get; }

    public T Value => OverridingValue?.Value ?? CombineValues(BaseValue, Modifiers.Select(m => m.Value));

    private static T CombineValues(T baseValue, IEnumerable<T> modifiers)
    {
        var result = baseValue;
        foreach (var modifier in modifiers)
        {
            result = (T)(object)((int)(object)result | (int)(object)modifier);
        }
        return result;
    }

    public EnumStat<T> Clone()
    {
        var clone = new EnumStat<T>(Name, BaseValue);
        clone.OverridingValue = OverridingValue?.Clone();
        clone.Modifiers.AddRange(Modifiers.Select(m => m.Clone()));
        return clone;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
