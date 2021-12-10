using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public static FieldController controller;

    [SerializeField]
    private Lamp[] lamps = new Lamp[3];
    [SerializeField]
    private Door[] doors = new Door[2];
    [SerializeField]
    private StartButton startButton;
    [SerializeField]
    private FieldSpawner fieldSpawner;

    private FieldGenerator fieldGenerator;
    private Field generatedField;

    private bool isTileMoving = false;
    private bool isPlaying = true;

    private void Awake()
    {
        if (controller == null) controller = this;
    }

    public void StartNewGame() 
    {
        ReinitializeLamps();
        fieldGenerator = new FieldGenerator();
        generatedField = fieldGenerator.GenerateField();
        fieldSpawner.SpawnNewTiles(generatedField);
        OpenDoors();
        isPlaying = true;
    }

    private void ReinitializeLamps()
    {
        foreach (Lamp lamp in lamps)
            lamp.Reinitialize();
    }

    private void UpdateLamps()
    {
        foreach (Lamp lamp in lamps)
            lamp.OnTilesMoved(generatedField);
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
            door.Open();
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
            door.Close();
    }

    private bool IsWon()
    {
        foreach (Lamp lamp in lamps)
            if (!lamp.IsTurnedOn) return false;

        return true;
    }

    private void OnWon()
    {
        isPlaying = false;
        SoundManager.manager.Play("winSound");
        CloseDoors();
        startButton.SetActive();
    }

    private void OnTileMoved(Tile tile, TileMovementDirection direction)
    {
        isTileMoving = true;
        generatedField.MoveTile(tile.tilePosition, direction);
        tile.MoveInDirection(direction);
    }

    public void OnTileStopped()
    {
        UpdateLamps();
        isTileMoving = false;
        if (IsWon()) OnWon();
    }

    public void OnTileDrag(Tile tile, TileMovementDirection direction)
    {
        if (isTileMoving || !isPlaying) return;

        bool canMove = generatedField.CanMove(tile.tilePosition, direction);

        if (canMove)
            OnTileMoved(tile, direction);

        Debug.Log(generatedField.ToString());
    }
}
