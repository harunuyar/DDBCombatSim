namespace DDBCombatSim.Combat;

public class CombatInstance
{
    public CombatInstance(CombatContext combatContext)
    {
        Context = combatContext;
    }

    public CombatContext Context { get; set; }
}
