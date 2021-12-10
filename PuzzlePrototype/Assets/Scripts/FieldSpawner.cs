using System.Collections.Generic;
using UnityEngine;

public class FieldSpawner : MonoBehaviour
{
    public static RectTransform _spawnPoint;

    [SerializeField]
    private RectTransform spawnPoint;
    [SerializeField]
    private TileData blockingTileData;
    [SerializeField]
    private TileData redTileData;
    [SerializeField]
    private TileData blueTileData;
    [SerializeField]
    private TileData yellowTileData;

    public static GameObject tileSample;
    private float tileSampleLength;

    private bool isSpawned = false;
    private List<GameObject> spawnedTiles = new List<GameObject>();

    private void Awake()
    {
        _spawnPoint = spawnPoint;
        tileSample = Resources.Load("Prefabs/TileSample") as GameObject;
    }

    // Sets the data for tile according to it's position;
    private void SetTileObjectData(Field field, GameObject tileObject, Vector2Int tilePosition)
    {
        Tile tile = tileObject.GetComponent<Tile>();
        switch (field[tilePosition]) {
            case TileType.BLOCK:
                tile.SetTileData(blockingTileData, tilePosition);
                break;
            case TileType.RED:
                tile.SetTileData(redTileData, tilePosition);
                break;
            case TileType.BLUE:
                tile.SetTileData(blueTileData, tilePosition);
                break;
            case TileType.YELLOW:
                tile.SetTileData(yellowTileData, tilePosition);
                break;
        }
    }

    public static Vector2 GetTilePositionOnScreen(Vector2Int tilePosition)
    {
        float tileSampleLength = tileSample.GetComponent<RectTransform>().rect.width;
        Vector2 offsetVector = new Vector2(tileSampleLength / 2, -tileSampleLength / 2);
        Vector2 position = (Vector2)_spawnPoint.localPosition + offsetVector +
            new Vector2((tilePosition.x - 1) * tileSampleLength, -(tilePosition.y - 1) * tileSampleLength);

        return position;
    }

    private void ClearField()
    {
        foreach (GameObject tile in spawnedTiles)
            Destroy(tile);

        spawnedTiles.Clear();
    }

    // Spawns tiles at the game scene. Clears the field previously, if necessary.
    public void SpawnNewTiles(Field field)
    {
        if (isSpawned) ClearField();

        for (int y = 1; y <= Field.FIELD_SIZE; ++y) {
            for (int x = 1; x <= Field.FIELD_SIZE; ++x) {
                Vector2Int tilePosition = new Vector2Int(x, y);

                if (field[tilePosition] == TileType.EMPTY) continue;

                Vector2 spawnPosition = GetTilePositionOnScreen(new Vector2Int(x, y));
                GameObject spawnedTile = Instantiate(tileSample, transform);
                spawnedTile.GetComponent<RectTransform>().localPosition = spawnPosition;
                SetTileObjectData(field, spawnedTile, tilePosition);
                spawnedTiles.Add(spawnedTile);
            }
        }

        isSpawned = true;
    }
}
