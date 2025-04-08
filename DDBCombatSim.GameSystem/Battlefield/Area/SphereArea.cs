namespace DDBCombatSim.GameSystem.Battlefield.Area;

using System.Numerics;

public class SphereAreaDefinition : IAreaDefinition
{
    public SphereAreaDefinition(float radius)
    {
        Radius = radius;
    }

    public EAreaType AreaType => EAreaType.Sphere;

    public float Radius { get; }

    public SphereArea Create(Vector2 center)
    {
        return new SphereArea(center, Radius);
    }
}

public class SphereArea : IArea
{
    public SphereArea(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vector2 Center { get; }

    public float Radius { get; }

    public bool Intersects(Tile tile, float tileSize)
    {
        // Same logic as CylinderArea (circle intersection)
        Vector2 tileCenter = tile.GetCenter(tileSize);
        float halfSize = tileSize / 2;

        float closestX = Math.Clamp(Center.X, tileCenter.X - halfSize, tileCenter.X + halfSize);
        float closestY = Math.Clamp(Center.Y, tileCenter.Y - halfSize, tileCenter.Y + halfSize);

        Vector2 closestPoint = new Vector2(closestX, closestY);
        float distanceSquared = Center.DistanceSquaredTo(closestPoint);

        return distanceSquared <= Radius * Radius;
    }
}
