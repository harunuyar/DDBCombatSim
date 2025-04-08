namespace DDBCombatSim.GameSystem.Signal;

public class ConnectionRecord
{
    public string ConnectionId { get; set; } = null!;
    public string? CharacterId { get; set; } = null!;
    public string EncounterId { get; set; } = null!;
    public bool IsDm { get; set; }
}
