namespace DDBCombatSim.GameSystem.Request.ResponseModels;

using DDBCombatSim.GameSystem.Battlefield;

public class TurnActionResponse
{
    public string? ActionId { get; set; } = null!;
    public Position? TargetPosition { get; set; } = null!;
}
