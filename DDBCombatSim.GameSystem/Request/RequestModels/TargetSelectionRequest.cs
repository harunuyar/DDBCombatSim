namespace DDBCombatSim.GameSystem.Request.RequestModels;

public class TargetSelectionRequest
{
    public IEnumerable<string> Options { get; set; } = null!;
    public int? Limit { get; set; }
}
