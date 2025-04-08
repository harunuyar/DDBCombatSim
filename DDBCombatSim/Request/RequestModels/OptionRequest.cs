namespace DDBCombatSim.Request.RequestModels;

public class OptionRequest
{
    string Name { get; set; } = null!;
    string Description { get; set; } = null!;
    IEnumerable<string> Options { get; set; } = null!;
}
