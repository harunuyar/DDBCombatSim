namespace DDBCombatSim.GameSystem.Battlefield;

public class Path
{
    public Position[] Steps { get; set; } = [];

    public int GetCost()
    {
        int cost = 0;

        for (var i = 1; i < Steps.Length; i++)
        {
            var distance = Steps[i - 1].DistanceTo(Steps[i]);
            cost += distance;
        }

        return cost;
    }
}
