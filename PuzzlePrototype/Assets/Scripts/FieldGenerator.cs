using System;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
class FieldGenerator
{
    private System.Random random = new System.Random();

    // The position of the tile are two positive integers corresponding to the coordinates. 
    // The origin is in the upper left corner.

    // Positions where blocking tiles are located.
    private List<Vector2Int> blockingTilesPositions = new List<Vector2Int>() { 
        new Vector2Int(2, 1), new Vector2Int(2, 3), new Vector2Int(2, 5),
        new Vector2Int(4, 1), new Vector2Int(4, 3), new Vector2Int(4, 5)
    };

    private List<Vector2Int> emptyTilesPositions = new List<Vector2Int>() {
        new Vector2Int(2, 2), new Vector2Int(2, 4),
        new Vector2Int(4, 2), new Vector2Int(4, 4)
    };

    private List<Vector2Int> movingTilesPositions = new List<Vector2Int>();
    private Field field = new Field();

    // Initializes positions where the moving tiles are located.
    private void InitializeMovingTilesPositions() 
    {
        for (int i = 1; i <= 5; i += 2)
            for (int j = 1; j <= 5; ++j)
                movingTilesPositions.Add(new Vector2Int(i, j));
    }

    // Adds tiles of the given type at given positions. 
    private void AddTilesOfType(TileType tilesType, List<Vector2Int> tilesPositions)
    {
        foreach (Vector2Int tilePosition in tilesPositions)
            field[tilePosition] = tilesType;
    }

    // Checks if one of the columns is filled with one type of tiles.
    private bool HaveFilledColumn()
    {
        foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
            for (int i = 1; i <= Field.FIELD_SIZE; i += 2)
                if (field.IsColumnOfType(i, tileType))
                    return true;

        return false;
    }

    // Generates the field object describing the types of tiles and their positions.
    public Field GenerateField()
    {
        InitializeMovingTilesPositions();
        AddTilesOfType(TileType.BLOCK, blockingTilesPositions);
        AddTilesOfType(TileType.EMPTY, emptyTilesPositions);
        do AddMovingTiles();
        while (HaveFilledColumn());

        return field;
    }

    // Randomly chooses the element in the list and returns it after removing it from the list.
    private T GetRandomElementFromList<T>(List<T> list) 
    {
        int chosenIndex = random.Next(list.Count);
        T element = list[chosenIndex];
        list.RemoveAt(chosenIndex);

        return element;
    }

    // Randomly generates the location of the moving tiles and adds them.
    private void AddMovingTiles() 
    {
        List<TileType> freeTileTypes = new List<TileType>();
        List<TileType> movingTilesTypes = new List<TileType> { TileType.RED, TileType.BLUE, TileType.YELLOW };
        for (int i = 0; i < Field.FIELD_SIZE; ++i) {
            freeTileTypes.AddRange(movingTilesTypes);
        }

        foreach (Vector2Int position in movingTilesPositions) {
            TileType chosenTileType = GetRandomElementFromList<TileType>(freeTileTypes);
            field[position] = chosenTileType;
        }

        Debug.Log(field.ToString());
    }
}
