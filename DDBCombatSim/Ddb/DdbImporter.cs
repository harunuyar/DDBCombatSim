namespace DDBCombatSim.Ddb;

using DDBCombatSim.GameSystem.Combat;
using DDBCombatSim.GameSystem.Combatant;
using DDBCombatSim.GameSystem.Signal;

public class DdbImporter
{
    private readonly HttpClient httpClient;
    private readonly CombatHub combatHub;

    public DdbImporter(HttpClient httpClient, CombatHub combatHub)
    {
        this.httpClient = httpClient;
        this.combatHub = combatHub;
    }

    public Task<PlayerCharacter> CreatePlayerCharacter(string id)
    {
        string url = "https://character-service.dndbeyond.com/character/v5/character/" + id;

        // TODO: Implement this method

        return Task.FromResult<PlayerCharacter>(null!);
    }

    public Task<CombatInstance> CreateCombatInstance(string encounterId)
    {
        string url = "https://encounter-service.dndbeyond.com/v1/encounters/" + encounterId;

        // TODO: Implement this method

        var context = new CombatContext(combatHub, encounterId, []);

        return Task.FromResult(new CombatInstance(context));
    }
}
