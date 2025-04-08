namespace DDBCombatSim.GameSystem.Battlefield;

using System.Drawing;

public interface IBattlefieldObject
{
    // Represents the position of the object on the battlefield.
    // If the object occupies multiple tiles, this should be the bottom-left tile position.
    Position Position { get; }
    ECoverType CoverType { get; }
    int Height { get; }
    int Width { get; }
    HashSet<Tile> OccupiedTiles { get; }

    public int GetDistanceTo(IBattlefieldObject other)
    {
        int leftA = Position.X;
        int rightA = Position.X + Width - 1;
        int topA = Position.Y;
        int bottomA = Position.Y + Height - 1;

        int leftB = other.Position.X;
        int rightB = other.Position.X + other.Width - 1;
        int topB = other.Position.Y;
        int bottomB = other.Position.Y + other.Height - 1;

        int dx = 0;
        if (rightA < leftB)
        {
            dx = leftB - rightA;
        }
        else if (rightB < leftA)
        {
            dx = leftA - rightB;
        }

        int dy = 0;
        if (bottomA < topB)
        {
            dy = topB - bottomA;
        }
        else if (bottomB < topA)
        {
            dy = topA - bottomB;
        }

        return Math.Max(dx, dy) * 5;
    }

    public bool IsWithinRange(IBattlefieldObject other, int rangeFeet)
    {
        return GetDistanceTo(other) <= rangeFeet;
    }
}
