using UnityEngine.UI;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private RectTransform startPoint;
    [SerializeField]
    private RectTransform endPoint;
    
    private RectTransform rectTransform;
    private Image image;
    private Vector2 targetPosition;
    private Vector2 movementDireciton = Vector2.right;

    private bool isMoving = false;
    private bool isOpening = false;

    private const float fillMargin = 0.15f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        FillInvisiblePart();
    }

    private void Move()
    {
        MovementUtils.MoveToPoint(rectTransform, targetPosition, movementSpeed, Time.deltaTime);
    }

    private void OnStopMoving()
    {
        rectTransform.localPosition = targetPosition;
        isMoving = false;
    }

    private void FillInvisiblePart()
    {
        image.fillAmount = fillMargin +
            MovementUtils.FindRatioOfPoint(rectTransform.localPosition, startPoint.localPosition, endPoint.localPosition);
    }

    public void Update()
    {
        if (isMoving) {
            FillInvisiblePart();
            Move();
            if (MovementUtils.IsReachedTargetPosition(rectTransform.localPosition, targetPosition, movementDireciton)) {
                OnStopMoving();
            }
        }
    }

    public void Close()
    {
        if (!isOpening) return;

        SoundManager.manager.Play("doorOpenSound");
        targetPosition = endPoint.localPosition;
        movementDireciton = Vector2.right * rectTransform.localScale.x;
        isOpening = false;
        isMoving = true;
    }

    public void Open()
    {
        if (isOpening) return;

        SoundManager.manager.Play("doorCloseSound");
        targetPosition = startPoint.localPosition;
        movementDireciton = Vector2.left * rectTransform.localScale.x;
        isOpening = true;
        isMoving = true;
    }
}
