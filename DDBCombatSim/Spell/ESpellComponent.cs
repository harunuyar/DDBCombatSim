namespace DDBCombatSim.Spell;

public enum ESpellComponent
{
    Verbal = 1,
    Somatic = 2,
    Material = 4
}

public static class ESpellComponentExtensions
{
    public static bool IsVerbal(this ESpellComponent component)
    {
        return component.HasFlag(ESpellComponent.Verbal);
    }

    public static bool IsSomatic(this ESpellComponent component)
    {
        return component.HasFlag(ESpellComponent.Somatic);
    }

    public static bool IsMaterial(this ESpellComponent component)
    {
        return component.HasFlag(ESpellComponent.Material);
    }

    public static string ToShortString(this ESpellComponent component)
    {
        return Enum.GetValues<ESpellComponent>()
            .Where(c => component.HasFlag(c)).Select(c => c.ToString().Substring(0, 1))
            .Aggregate((a, b) => $"{a}/{b}");
    }
}
