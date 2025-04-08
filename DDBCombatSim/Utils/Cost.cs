namespace DDBCombatSim.Utils;

public class Cost
{
    public ECostType Type { get; set; }
    public int Amount { get; set; }

    public bool IsTurnAction()
    {
        return Type == ECostType.Action || Type == ECostType.BonusAction;
    }

    public bool IsAction()
    {
        return Type == ECostType.Action;
    }

    public bool IsBonusAction()
    {
        return Type == ECostType.BonusAction;
    }

    public bool IsReaction()
    {
        return Type == ECostType.Reaction;
    }

    public bool IsFreeAction()
    {
        return Type == ECostType.FreeAction;
    }

    public bool IsMovement()
    {
        return Type == ECostType.Movement;
    }

    public bool IsNonCombat()
    {
        return Type == ECostType.NonCombat;
    }
}

public enum ECostType
{
    Action,
    BonusAction,
    Reaction,
    FreeAction,
    Movement,
    NonCombat
}