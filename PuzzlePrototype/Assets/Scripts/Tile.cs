using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public enum TileType
{
    RED, BLUE, YELLOW, BLOCK, EMPTY
}

public enum TileMovementDirection
{
    UP, DOWN, LEFT, RIGHT
}

public class Tile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public TileType tileType { get; private set; } = TileType.EMPTY;
    [HideInInspector]
    public Vector2Int tilePosition { get; private set; } = Vector2Int.zero;

    [SerializeField]
    private float movementSpeed = 0f;
    [SerializeField]
    private float dragThreshold = 40f;

    private bool isMoving = false;
    private bool isDragPossible = true;

    private Vector2 targetPosition = Vector2.zero; // The position where the tile moves when moving.
    private RectTransform rectTransform;
    private float tileSampleLength;
    private Vector2 lastPointerPosition = Vector2.zero;

    private TileMovementDirection tileMovementDirection;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tileSampleLength = FieldSpawner.tileSample.GetComponent<RectTransform>().rect.width;
    }

    public static Vector2Int TileDirectionToVector(TileMovementDirection tileDirection)
    {
        switch (tileDirection) {
            case TileMovementDirection.LEFT:
                return Vector2Int.left;
            case TileMovementDirection.RIGHT:
                return Vector2Int.right;
            case TileMovementDirection.UP:
                return Vector2Int.up;
            default:
                return Vector2Int.down;
        }
    }

    public void SetTileData(TileData tileData, Vector2Int tilePosition)
    {
        GetComponent<Image>().sprite = tileData.tileSprite;
        tileType = tileData.tileType;
        this.tilePosition = tilePosition;
    }

    private void OnStopMoving()
    {
        isMoving = false;
        rectTransform.localPosition = targetPosition;
        FieldController.controller.OnTileStopped();
    }

    private void Update()
    {
        if (tileType != TileType.BLOCK && isMoving) {
            MovementUtils.MoveToPoint(rectTransform, targetPosition, movementSpeed, Time.deltaTime);
            bool isReachedTargetPosition = MovementUtils.IsReachedTargetPosition(
                rectTransform.localPosition, targetPosition, Tile.TileDirectionToVector(tileMovementDirection));
            if (isReachedTargetPosition) OnStopMoving();
        }
    }

    public void MoveInDirection(TileMovementDirection direction)
    {
        tilePosition = Field.GetTargetPosition(tilePosition, direction);
        targetPosition = MovementUtils.GetTilePositionOnScreen(tilePosition);
        isMoving = true;
        SoundManager.manager.Play("dragSound");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (tileType == TileType.BLOCK) return;

        lastPointerPosition = eventData.position;
    }

    // Finds the nearest direction acorrding to the angle.
    private TileMovementDirection AngleToNearestDirection(float angle)
    {
        if (angle >= 45f && angle < 135f) return TileMovementDirection.UP;
        if (angle >= 135f || angle < -135f) return TileMovementDirection.LEFT;
        if (angle >= -135f && angle < -45f) return TileMovementDirection.DOWN;
        return TileMovementDirection.RIGHT;
    }

    // Finds the movement direction for the tile according the pointer movement direction.
    private TileMovementDirection GetTileMovementDirection(Vector2 pointerDragVector)
    {
        // The angle between the direction of the pointer and the positive direction of the abcissa axis.
        float directionAngle = Vector2.SignedAngle(Vector2.right, pointerDragVector);
        return AngleToNearestDirection(directionAngle);
    }

    private bool CanMove() => isDragPossible && !isMoving && tileType != TileType.BLOCK;

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanMove()) return;

        Vector2 currentPointerPosition = eventData.position;
        Vector2 pointerDragVector = currentPointerPosition - lastPointerPosition;

        if (pointerDragVector.magnitude < dragThreshold) return;

        tileMovementDirection = GetTileMovementDirection(pointerDragVector);
        FieldController.controller.OnTileDrag(this, tileMovementDirection);
        isDragPossible = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragPossible = true;
    }
}
