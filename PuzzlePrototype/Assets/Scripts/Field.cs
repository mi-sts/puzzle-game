using UnityEngine;

[System.Serializable]
public class Field
{
    public const int FIELD_SIZE = 5;
    public TileType[,] Tiles { get; private set; } = new TileType[FIELD_SIZE, FIELD_SIZE];

    public Field()
    {
        for (int i = 0; i < FIELD_SIZE; ++i)
            for (int j = 0; j < FIELD_SIZE; ++j) {
                Tiles[i, j] = TileType.EMPTY;
            }
    }

    public override string ToString()
    {
        string fieldString = "";
        for (int y = 1; y <= FIELD_SIZE; ++y) {
            for (int x = 1; x <= FIELD_SIZE; ++x) {
                char tileSymbol = ' ';
                switch (this[new Vector2Int(x, y)]) {
                    case TileType.EMPTY:
                        tileSymbol = '_';
                        break;
                    case TileType.BLOCK:
                        tileSymbol = '#';
                        break;
                    case TileType.RED:
                        tileSymbol = 'R';
                        break;
                    case TileType.BLUE:
                        tileSymbol = 'B';
                        break;
                    case TileType.YELLOW:
                        tileSymbol = 'Y';
                        break;
                }

                fieldString += tileSymbol;
            }
            fieldString += '\n';
        }

        return fieldString;
    }

    private bool IsIndexCorrect(int index) => index >= 1 && index <= FIELD_SIZE;
    private bool IsPositionCorrect(Vector2Int position) => IsIndexCorrect(position.x) && IsIndexCorrect(position.y);

    public TileType this[Vector2Int position]
    {
        get
        {
            if (IsPositionCorrect(position))
                return Tiles[position.x - 1, position.y - 1];

            Debug.LogError("The tile index does not exist!");
            return TileType.EMPTY;
        }

        set
        {
            if (IsPositionCorrect(position))
                Tiles[position.x - 1, position.y - 1] = value;
            else
                Debug.LogError("The tile index does not exist!");
        }
    }

    public bool IsEmptyTile(Vector2Int tilePosition) => this[tilePosition] == TileType.EMPTY;

    public bool IsBlockingTile(Vector2Int tilePosition) => this[tilePosition] == TileType.BLOCK;

    public static Vector2Int GetTargetPosition(Vector2Int tilePosition, TileMovementDirection movementDirection)
    {
        Vector2Int movementVector = Tile.TileDirectionToVector(movementDirection);
        movementVector.y *= -1; // Since the direction of the ordinate axes in the editor and in the indexing of tiles is opposite.
        Vector2Int targetPosition = tilePosition + movementVector;

        return targetPosition;
    }

    // Ñhecks if it is possible to move a tile.
    public bool CanMove(Vector2Int tilePosition, TileMovementDirection movementDirection)
    {
        Vector2Int targetPosition = GetTargetPosition(tilePosition, movementDirection);
        if (!IsPositionCorrect(tilePosition) || !IsPositionCorrect(targetPosition) || IsEmptyTile(tilePosition) ||
            !IsEmptyTile(targetPosition) || IsBlockingTile(tilePosition)) {
            return false;
        }
        return true;
    }

    // Moves the tile according the direction.
    public void MoveTile(Vector2Int tilePosition, TileMovementDirection movementDirection)
    {
        if (!CanMove(tilePosition, movementDirection)) {
            Debug.LogWarning("Can't move the tile at that position.");
            return;
        }
        Vector2Int targetPosition = GetTargetPosition(tilePosition, movementDirection);

        this[targetPosition] = this[tilePosition];
        this[tilePosition] = TileType.EMPTY;
    }

    // Cheks if the column of given type;
    public bool IsColumnOfType(int columnNumber, TileType type)
    {
        for (int y = 1; y <= Field.FIELD_SIZE; ++y) {
            Vector2Int tilePosition = new Vector2Int(columnNumber, y);
            if (this[tilePosition] != type) {
                return false;
            }
        }

        return true;
    }
}
