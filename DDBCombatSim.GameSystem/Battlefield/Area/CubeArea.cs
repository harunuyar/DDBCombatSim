namespace DDBCombatSim.GameSystem.Battlefield.Area;

using System.Numerics;

public class CubeAreaDefinition : IAreaDefinition
{
    public CubeAreaDefinition(float size)
    {
        Size = size;
    }

    public EAreaType AreaType => EAreaType.Cube;

    public float Size { get; }

    public CubeArea Create(Vector2 center)
    {
        return new CubeArea(center, Size);
    }
}

public class CubeArea : IArea
{
    public CubeArea(Vector2 center, float size)
    {
        Center = center;
        Size = size;
    }

    public Vector2 Center { get; }

    public float Size { get; }

    public bool Intersects(Tile tile, float tileSize)
    {
        float halfSize = Size / 2;
        Vector2 tileCenter = tile.GetCenter(tileSize);
        float halfTile = tileSize / 2;

        bool overlapX = Math.Abs(tileCenter.X - Center.X) <= (halfSize + halfTile);
        bool overlapY = Math.Abs(tileCenter.Y - Center.Y) <= (halfSize + halfTile);

        return overlapX && overlapY;
    }
}
