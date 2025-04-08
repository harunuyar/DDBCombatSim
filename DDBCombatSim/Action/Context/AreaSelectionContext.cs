namespace DDBCombatSim.Action.Context;

using DDBCombatSim.Battlefield.Area;

public class AreaSelectionContext
{
    public IAreaDefinition AreaDefinition { get; set; } = null!;
    public bool ControlledArea { get; set; }
    public int? TargetLimit { get; set; }
    public int Range { get; set; }
}