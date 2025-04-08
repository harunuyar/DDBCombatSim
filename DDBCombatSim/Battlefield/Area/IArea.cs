namespace DDBCombatSim.Battlefield.Area;

using System.Numerics;

public interface IAreaDefinition
{
    EAreaType AreaType { get; }
}

public interface IArea
{
    bool Intersects(Tile tile, float tileSize = 5f);
}

public static class Vector2Extensions
{
    public static float DistanceSquaredTo(this Vector2 vector, Vector2 other)
    {
        float dx = vector.X - other.X;
        float dy = vector.Y - other.Y;
        return dx * dx + dy * dy;
    }
}