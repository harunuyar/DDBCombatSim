namespace DDBCombatSim.GameSystem.Action.Context;

using DDBCombatSim.GameSystem.Battlefield.Area;

public class AreaSelectionContext
{
    public IAreaDefinition AreaDefinition { get; set; } = null!;
    public bool ControlledArea { get; set; }
    public int? TargetLimit { get; set; }
    public int Range { get; set; }
}