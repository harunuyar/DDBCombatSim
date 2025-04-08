namespace DDBCombatSim.GameSystem.Utils;

public enum EWeaponCategory
{
    None = 0,
    SimpleMelee = 1,
    SimpleRanged = 2,
    MartialMelee = 3,
    MartialRanged = 4,
    Unarmed = 5,
    Natural = 6,
    Improvised = 7
}

public static class EWeaponCategoryExtensions
{
    public static bool IsMelee(this EWeaponCategory weaponCategory)
    {
        return weaponCategory == EWeaponCategory.SimpleMelee
            || weaponCategory == EWeaponCategory.MartialMelee
            || weaponCategory == EWeaponCategory.Unarmed
            || weaponCategory == EWeaponCategory.Improvised;
    }

    public static bool IsRanged(this EWeaponCategory weaponCategory)
    {
        return weaponCategory == EWeaponCategory.SimpleRanged 
            || weaponCategory == EWeaponCategory.MartialRanged
            || weaponCategory == EWeaponCategory.Improvised;
    }

    public static bool IsSimple(this EWeaponCategory weaponCategory)
    {
        return weaponCategory == EWeaponCategory.SimpleMelee 
            || weaponCategory == EWeaponCategory.SimpleRanged;
    }

    public static bool IsMartial(this EWeaponCategory weaponCategory)
    {
        return weaponCategory == EWeaponCategory.MartialMelee 
            || weaponCategory == EWeaponCategory.MartialRanged;
    }
}
