namespace DDBCombatSim.Predefined.Effects.Conditions;

using DDBCombatSim.Effect;

public class Deafened : BaseEffect
{
    public Deafened() : base("Deafened")
    {
    }

    public override int Priority => 0;
}
