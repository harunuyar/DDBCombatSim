namespace DDBCombatSim.GameSystem.Battlefield;

public class Battlefield
{
    private readonly Tile[][] tileMap;

    public Battlefield(int width, int height)
    {
        tileMap = new Tile[width][];
        for (int i = 0; i < width; i++)
        {
            tileMap[i] = new Tile[height];
            for (int j = 0; j < height; j++)
            {
                tileMap[i][j] = new Tile(i, j);
            }
        }
    }

    public IEnumerable<Tile> Tiles => tileMap.SelectMany(x => x);

    public IEnumerable<IBattlefieldObject> GetObjectsAt(Position position)
    {
        return GetObjectsAt(position.X, position.Y);
    }

    public IEnumerable<IBattlefieldObject> GetObjectsAt(int x, int y)
    {
        if (!IsPositionWithinBounds(x, y, 1, 1))
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Position is out of bounds");
        }

        return tileMap[x][y].Objects;
    }

    public void SetObjectPosition(IBattlefieldObject obj, Position position)
    {
        if (!IsPositionWithinBounds(position, obj.Width, obj.Height))
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of bounds");
        }
        if (IsPositionOccupied(position, obj.Width, obj.Height))
        {
            throw new InvalidOperationException("Position is already occupied");
        }

        foreach (var tile in obj.OccupiedTiles)
        {
            tile.Objects.Remove(obj);
        }

        obj.OccupiedTiles.Clear();

        for (int x = 0; x < obj.Width; x++)
        {
            for (int y = 0; y < obj.Height; y++)
            {
                var tile = tileMap[position.X + x][position.Y + y];
                tile.Objects.Add(obj);
                obj.OccupiedTiles.Add(tile);
            }
        }

        obj.Position.SetPosition(position);
    }

    public bool IsPositionOccupied(Position position, int width, int height)
    {
        return IsPositionOccupied(position.X, position.Y, width, height);
    }

    public bool IsPositionOccupied(int x, int y, int width, int height)
    {
        if (!IsPositionWithinBounds(x, y, width, height))
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Position is out of bounds");
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileMap[x + i][y + j].IsOccupied)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsPositionBlocked(Position position, int width, int height)
    {
        return IsPositionBlocked(position.X, position.Y, width, height);
    }

    public bool IsPositionBlocked(int x, int y, int width, int height)
    {
        if (!IsPositionWithinBounds(x, y, width, height))
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Position is out of bounds");
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileMap[x + i][y + j].Type == ETileType.Blocked)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsPositionDifficultTerrain(Position position, int width, int height)
    {
        return IsPositionDifficultTerrain(position.X, position.Y, width, height);
    }

    public bool IsPositionDifficultTerrain(int x, int y, int width, int height)
    {
        if (!IsPositionWithinBounds(x, y, width, height))
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Position is out of bounds");
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileMap[x + i][y + j].Type == ETileType.DifficultTerrain)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsPositionWithinBounds(Position position, int width, int height)
    {
        return IsPositionWithinBounds(position.X, position.Y, width, height);
    }

    public bool IsPositionWithinBounds(int x, int y, int width, int height)
    {
        return x >= 0 && x + width <= tileMap.Length && y >= 0 && y + height <= tileMap[0].Length;
    }

    public void RemoveObject(IBattlefieldObject obj)
    {
        foreach (var tile in obj.OccupiedTiles)
        {
            tile.Objects.Remove(obj);
        }

        obj.OccupiedTiles.Clear();
        obj.Position.SetPosition(-1, -1);
    }

    public bool IsPathClear(IBattlefieldObject obj, Path path)
    {
        if (path.Steps.Length == 0)
        {
            return false;
        }

        // temporarily remove object from occupied tiles
        foreach (var tile in obj.OccupiedTiles)
        {
            tile.Objects.Remove(obj);
        }

        void ReAddObjectToOccupiedTiles()
        {
            foreach (var tile in obj.OccupiedTiles)
            {
                tile.Objects.Add(obj);
            }
        }

        foreach (var step in path.Steps)
        {
            if (!IsPositionWithinBounds(step, obj.Width, obj.Height) || IsPositionOccupied(step, obj.Width, obj.Height))
            {
                ReAddObjectToOccupiedTiles();
                return false;
            }
        }

        var currentPosition = path.Steps[0];
        foreach (var nextPosition in path.Steps.Skip(1))
        {
            if (!currentPosition.IsAdjacentTo(nextPosition))
            {
                ReAddObjectToOccupiedTiles();
                return false;
            }

            // Check for diagonal movement and don't allow it if it would cut through occupied positions
            // | X |   |
            // | O | X |

            if (IsPositionOccupied(currentPosition.X, nextPosition.Y, obj.Width, obj.Height) && IsPositionOccupied(nextPosition.X, currentPosition.Y, obj.Width, obj.Height))
            {
                ReAddObjectToOccupiedTiles();
                return false;
            }
        }

        ReAddObjectToOccupiedTiles();
        return true;
    }
}
