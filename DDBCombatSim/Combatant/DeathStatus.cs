namespace DDBCombatSim.Combatant;

public class DeathStatus
{
    public int Successes { get; set; }
    public int Failures { get; set; }
    public bool IsDying { get; set; }
    public bool IsDead { get; set; }

    public DeathStatus()
    {
        Successes = 0;
        Failures = 0;
        IsDying = false;
        IsDead = false;
    }

    public void Reset()
    {
        Successes = 0;
        Failures = 0;
        IsDying = false;
        IsDead = false;
    }

    public void AddSuccess(int amount)
    {
        Successes += amount;
        if (Successes >= 3)
        {
            Reset();
        }
    }

    public void AddFailure(int amount)
    {
        Failures += amount;
        if (Failures >= 3)
        {
            Reset();
            IsDead = true;
        }
    }
}
