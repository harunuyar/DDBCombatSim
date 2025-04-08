namespace DDBCombatSim.Request.ResponseModels;

using DDBCombatSim.Battlefield;

public class TurnActionResponse
{
    public string? ActionId { get; set; } = null!;
    public Position? TargetPosition { get; set; } = null!;
}
