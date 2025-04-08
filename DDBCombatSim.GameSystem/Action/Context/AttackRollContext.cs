namespace DDBCombatSim.GameSystem.Action.Context;

using DDBCombatSim.GameSystem.Utils;

public class AttackRollContext : RollContext
{
    public AttackRollContext()
    {
        Dice = new Dice(1, 20);
    }

    public ERange Range { get; set; }
    public EAttackSourceType SourceType { get; set; }
}
