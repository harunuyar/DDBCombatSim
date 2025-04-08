namespace DDBCombatSim.GameSystem.Battlefield.Area;

using System.Numerics;

public class LineAreaDefinition : IAreaDefinition
{
    public LineAreaDefinition(float length)
    {
        Length = length;
    }

    public EAreaType AreaType => EAreaType.Line;

    public float Length { get; }

    public LineArea Create(Vector2 start, float directionDegrees)
    {
        return new LineArea(start, Length, directionDegrees);
    }
}

public class LineArea : IArea
{
    public LineArea(Vector2 start, float length, float directionDegrees)
    {
        Start = start;
        Length = length;
        DirectionDegrees = directionDegrees;
    }

    public float Width { get; } = 5f; // Default width is 5ft per D&D official rules
    public Vector2 Start { get; }
    public float Length { get; }
    public float DirectionDegrees { get; }

    public bool Intersects(Tile tile, float tileSize)
    {
        Vector2 tileCenter = tile.GetCenter(tileSize);
        Vector2 dirVector = new Vector2(MathF.Cos(DirectionDegrees * MathF.PI / 180), MathF.Sin(DirectionDegrees * MathF.PI / 180));
        Vector2 end = new Vector2(Start.X + dirVector.X * Length, Start.Y + dirVector.Y * Length);

        float distance = DistancePointToLineSegment(tileCenter, Start, end);
        return distance <= (Width / 2 + tileSize / 2);
    }

    private float DistancePointToLineSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        float l2 = a.DistanceSquaredTo(b);
        if (l2 == 0.0) return MathF.Sqrt(p.DistanceSquaredTo(a));

        float t = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / l2;
        t = Math.Clamp(t, 0, 1);

        Vector2 projection = new Vector2(a.X + t * (b.X - a.X), a.Y + t * (b.Y - a.Y));
        return MathF.Sqrt(p.DistanceSquaredTo(projection));
    }
}
