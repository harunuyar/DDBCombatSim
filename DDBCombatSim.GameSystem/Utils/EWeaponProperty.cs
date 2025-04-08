namespace DDBCombatSim.GameSystem.Utils;

public enum EWeaponProperty
{
    None = 0,
    Ammunition = 1 << 0,
    Finesse = 1 << 1,
    Heavy = 1 << 2,
    Light = 1 << 3,
    Loading = 1 << 4,
    Monk = 1 << 5,
    Reach = 1 << 6,
    Special = 1 << 7,
    Thrown = 1 << 8,
    TwoHanded = 1 << 9,
    Versatile = 1 << 10
}
