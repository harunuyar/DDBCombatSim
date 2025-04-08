namespace DDBCombatSim.Combat;

using DDBCombatSim.Ddb;
using System.Collections.Concurrent;

public class CombatManager
{
    private readonly ConcurrentDictionary<string, CombatInstance> combats = new();
    private readonly DdbImporter ddbImporter;

    public CombatManager(DdbImporter ddbImporter)
    {
        this.ddbImporter = ddbImporter;
    }

    public async Task<CombatInstance> GetOrCreateCombat(string encounterId)
    {
        if (!combats.TryGetValue(encounterId, out var combat))
        {
            combat = await ddbImporter.CreateCombatInstance(encounterId);
            combats.TryAdd(encounterId, combat);
        }

        return combat;
    }

    public CombatInstance? GetCombat(string encounterId)
    {
        return combats.TryGetValue(encounterId, out var combat) ? combat : null;
    }
}
