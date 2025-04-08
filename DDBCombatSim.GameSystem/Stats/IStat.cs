namespace DDBCombatSim.GameSystem.Stats;

public interface IStat<T> : IDndObject where T : struct
{
    T BaseValue { get; set; }
    Modifier<T>? OverridingValue { get; set; }
    List<Modifier<T>> Modifiers { get; }
    T Value { get; }
}
