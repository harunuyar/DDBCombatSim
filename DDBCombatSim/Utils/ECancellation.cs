namespace DDBCombatSim.Utils;

public enum ECancellation
{
    None = 0,
    UserCancelled = 1,
    Error = 2,
    SystemCancelled = 4,
    Interrupted = 8,
    Partial = 16
}

public static class ECancellationExtensions
{
    public static bool ShouldStopAction(this ECancellation cancellation)
    {
        return cancellation.HasFlag(ECancellation.UserCancelled) 
            || cancellation.HasFlag(ECancellation.Error) 
            || cancellation.HasFlag(ECancellation.SystemCancelled) 
            || cancellation.HasFlag(ECancellation.Interrupted);
    }

    public static bool ShouldStopActionEvent(this ECancellation cancellation)
    {
        return cancellation != ECancellation.None;
    }

    public static bool ShouldRefund(this ECancellation cancellation)
    {
        return cancellation.HasFlag(ECancellation.UserCancelled)
            || cancellation.HasFlag(ECancellation.Error)
            || cancellation.HasFlag(ECancellation.SystemCancelled);
    }
}