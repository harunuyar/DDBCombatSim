namespace DDBCombatSim.Battlefield.Area;

using System.Numerics;

public class CylinderAreaDefinition : IAreaDefinition
{
    public CylinderAreaDefinition(float radius)
    {
        Radius = radius;
    }
    public EAreaType AreaType => EAreaType.Cylinder;

    public float Radius { get; }

    public CylinderArea Create(Vector2 center)
    {
        return new CylinderArea(center, Radius);
    }
}

public class CylinderArea : IArea
{
    public CylinderArea(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vector2 Center { get; }

    public float Radius { get; }

    public bool Intersects(Tile tile, float tileSize)
    {
        Vector2 tileCenter = tile.GetCenter(tileSize);
        float halfSize = tileSize / 2;

        float closestX = Math.Clamp(Center.X, tileCenter.X - halfSize, tileCenter.X + halfSize);
        float closestY = Math.Clamp(Center.Y, tileCenter.Y - halfSize, tileCenter.Y + halfSize);

        Vector2 closestPoint = new Vector2(closestX, closestY);
        float distanceSquared = Center.DistanceSquaredTo(closestPoint);

        return distanceSquared <= Radius * Radius;
    }
}
