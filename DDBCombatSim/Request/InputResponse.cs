namespace DDBCombatSim.Request;

public class InputResponse<T> 
{
    public string RequestId { get; set; } = null!;
    public EInputResponseType ResponseType { get; set; }
    public T Data { get; set; } = default!;
}
