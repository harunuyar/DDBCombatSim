﻿namespace DDBCombatSim.Request.RequestModels;

using DDBCombatSim.Stats;
using DDBCombatSim.Utils;

public class RollRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int DieCount { get; set; }
    public int DieSize { get; set; }
    public IntStat Modifier { get; set; } = null!;
    public EnumStat<EAdvantage> Advantage { get; set; } = null!;
    public Modifier<bool> Critical { get; set; } = null!;
    public IntStat Target { get; set; } = null!;
}
