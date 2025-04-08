namespace DDBCombatSim.GameSystem.Battlefield;

using System.Numerics;

public class Tile
{
    public Tile(int x, int y)
    {
        Position = new Position(x, y);
        Objects = [];
    }

    public Position Position { get; }

    public HashSet<IBattlefieldObject> Objects { get; set; }

    public bool IsOccupied => Objects.Count > 0;

    public ETileType Type { get; set; }

    public Vector2 GetCenter(float tileSize = 5)
    {
        return new Vector2(Position.X * tileSize + tileSize / 2, Position.Y * tileSize + tileSize / 2);
    }
}
