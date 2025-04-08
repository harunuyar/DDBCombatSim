namespace DDBCombatSim.GameSystem.Stats;

public class Modifier<T> : IDndObject, ICloneable
{
    public Modifier(IDndObject source, string name, T value)
    {
        Source = source;
        Name = name;
        Value = value;
    }

    public IDndObject Source { get; set; }

    public string Name { get; set; }

    public T Value { get; set; }

    public Modifier<T> Clone()
    {
        return new Modifier<T>(Source, Name, Value);
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
