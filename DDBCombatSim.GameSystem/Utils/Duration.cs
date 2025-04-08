namespace DDBCombatSim.GameSystem.Utils;

using DDBCombatSim.GameSystem.Stats;

public enum EDurationType
{
    Instantaneous,
    Time,
    Trigger,
    Forever,
    Special
}

public class Duration
{
    public Duration(EDurationType type, int? triggerLimit, TimeSpan? time)
    {
        Type = type;
        Time = time;
        IsFinished = false;

        if (type == EDurationType.Instantaneous)
        {
            triggerLimit = 1;
        }

        TriggerLimit = triggerLimit.HasValue ? new ConsumableStat(new IntStat("Trigger Limit", triggerLimit.Value)) : null;
    }

    public EDurationType Type { get; }

    public bool IsFinished { get; set; }

    ConsumableStat? TriggerLimit { get; }

    public TimeSpan? Time { get; set; }

    public void Trigger()
    {
        if (Type.HasFlag(EDurationType.Trigger) && TriggerLimit != null)
        {
            TriggerLimit.Use();
            if (TriggerLimit.Value == 0)
            {
                IsFinished = true;
            }
        }
    }

    public void PassTime(TimeSpan time)
    {
        if (Type.HasFlag(EDurationType.Time) && Time.HasValue)
        {
            Time -= time;
            if (Time.Value <= TimeSpan.Zero)
            {
                IsFinished = true;
            }
        }
    }

    public static Duration Instantaneous => new Duration(EDurationType.Instantaneous, null, null);

    public static Duration FromTime(TimeSpan time) => new Duration(EDurationType.Time, null, time);

    public static Duration FromTrigger(int triggerLimit) => new Duration(EDurationType.Trigger, triggerLimit, null);

    public static Duration Forever => new Duration(EDurationType.Forever, null, null);

    public static Duration Special => new Duration(EDurationType.Special, null, null);

    public Duration Copy()
    {
        return new Duration(Type, TriggerLimit?.BaseStat.Value, Time);
    }
}
