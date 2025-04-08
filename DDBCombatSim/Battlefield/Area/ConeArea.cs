namespace DDBCombatSim.Battlefield.Area;

using System.Numerics;

public class ConeAreaDefinition : IAreaDefinition
{
    public ConeAreaDefinition(float length)
    {
        Length = length;
    }

    public EAreaType AreaType => EAreaType.Cone;

    public float Length { get; }

    public ConeArea Create(Vector2 origin, float directionDegrees)
    {
        return new ConeArea(origin, Length, directionDegrees);
    }
}

public class ConeArea : IArea
{
    private const float ConeAngleDegrees = 53f; // Official approximate angle for D&D cones

    public ConeArea(Vector2 origin, float length, float directionDegrees)
    {
        Origin = origin;
        Length = length;
        DirectionDegrees = directionDegrees;
    }

    public Vector2 Origin { get; }

    public float Length { get; }

    public float DirectionDegrees { get; }

    public bool Intersects(Tile tile, float tileSize)
    {
        Vector2 tileCenter = tile.GetCenter(tileSize);
        Vector2 toTile = new Vector2(tileCenter.X - Origin.X, tileCenter.Y - Origin.Y);

        float distanceSquared = toTile.DistanceSquaredTo(new Vector2(0, 0));
        if (distanceSquared > Length * Length)
            return false;

        float angleToTile = MathF.Atan2(toTile.Y, toTile.X) * (180 / MathF.PI);
        float deltaAngle = NormalizeAngle(angleToTile - DirectionDegrees);

        return Math.Abs(deltaAngle) <= ConeAngleDegrees / 2;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        return angle;
    }
}
