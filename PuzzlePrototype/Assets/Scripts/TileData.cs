using UnityEngine;

[CreateAssetMenu(fileName = "NewTileData", menuName = "TileData", order = 1)]
public class TileData : ScriptableObject
{
    public Sprite tileSprite;
    public TileType tileType = TileType.EMPTY;
}
