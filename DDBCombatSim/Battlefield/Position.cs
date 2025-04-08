namespace DDBCombatSim.Battlefield;

using System.Numerics;

public struct Position
{
    private const int TileSize = 5;

    public int X { get; set; }
    public int Y { get; set; }

    public Position()
    {
        X = 0;
        Y = 0;
    }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetPosition(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public int DistanceTo(Position other)
    {
        return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y)) * TileSize;
    }

    public bool IsWithinRange(Position other, int rangeFeet)
    {
        return DistanceTo(other) <= rangeFeet;
    }

    public readonly bool IsAdjacentTo(Position other)
    {
        return !Equals(other) && Math.Abs(X - other.X) <= 1 && Math.Abs(Y - other.Y) <= 1;
    }

    public Vector2 GetCenterPoint()
    {
        return new Vector2(X * TileSize + TileSize / 2, Y * TileSize + TileSize / 2);
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is Position other && X == other.X && Y == other.Y;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}
