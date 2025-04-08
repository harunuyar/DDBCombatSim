namespace DDBCombatSim.Request;

public class InputRequest<TRequest>
{
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    public EInputRequestType RequestType { get; set; }
    public string CombatantId { get; set; } = null!;
    public TRequest Data { get; set; } = default!;
}
