﻿namespace DDBCombatSim.GameSystem.Utils;

public enum EDamageType
{
    None = 0,
    Acid = 1,
    Bludgeoning = 2,
    Cold = 4,
    Fire = 8,
    Force = 16,
    Lightning = 32,
    Necrotic = 64,
    Piercing = 128,
    Poison = 256,
    Psychic = 512,
    Radiant = 1024,
    Slashing = 2048,
    Thunder = 4096
}

public static class EDamageTypeExtensions
{
    public const EDamageType All = (EDamageType)(-1);
}