using UnityEngine;

[SerializeField]
public static class MovementUtils
{
    public static bool IsReachedTargetPosition(
        Vector2 objectPosition, Vector2 targetPosition, Vector2 movementDirection)
    {
        if (movementDirection == Vector2.left) 
            return objectPosition.x <= targetPosition.x;
        if (movementDirection == Vector2.right) 
            return objectPosition.x >= targetPosition.x;
        if (movementDirection == Vector2.up) 
            return objectPosition.y >= targetPosition.y;
        if (movementDirection == Vector2.down) 
            return objectPosition.y <= targetPosition.y;

        Debug.LogError("Incorrect direction of the movement!");
        return false;
    }

    public static void MoveToPoint(
        RectTransform objectRectTransform, Vector3 targetPosition, float movementSpeed, float deltaTime)
    {
        Vector3 movementDirection = (targetPosition - objectRectTransform.localPosition).normalized;
        objectRectTransform.localPosition += movementDirection * movementSpeed * deltaTime;
    }

    // Finds the ratio of the distance from start to point to distance from start to end.
    public static float FindRatioOfPoint(Vector2 pointPosition, Vector2 startPosition, Vector2 endPosition)
    {
        return Vector2.Distance(startPosition, pointPosition) / Vector2.Distance(startPosition, endPosition);
    }

    public static Vector2 GetTilePositionOnScreen(Vector2Int tilePosition)
    {
        float tileSampleLength = FieldSpawner.tileSample.GetComponent<RectTransform>().rect.width;
        Vector2 offsetVector = new Vector2(tileSampleLength / 2, -tileSampleLength / 2);
        Vector2 position = (Vector2)FieldSpawner._spawnPoint.localPosition + offsetVector +
            new Vector2((tilePosition.x - 1) * tileSampleLength, -(tilePosition.y - 1) * tileSampleLength);

        return position;
    }
}
