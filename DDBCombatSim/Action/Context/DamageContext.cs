namespace DDBCombatSim.Action.Context;

using DDBCombatSim.Utils;

public class DamageContext
{
    public int? ConstantDamage { get; set; }
    public EDamageType DamageType { get; set; }
    public bool HalfDamageOnSave { get; set; }
    public EMagicNature MagicNature { get; set; }
}
