using UnityEngine.UI;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    private int columnNumber = 1;
    [SerializeField]
    private TileType lampType;
    [SerializeField]
    private Sprite lampOnSprite;
    [SerializeField]
    private Sprite lampOffSprite;

    public bool IsTurnedOn { get; private set; } = false;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void TurnOffLamp()
    {
        IsTurnedOn = false;
        image.sprite = lampOffSprite;
        SoundManager.manager.Play("lampOffSound");
    }

    private void TurnOnLamp()
    {
        IsTurnedOn = true;
        image.sprite = lampOnSprite;
        SoundManager.manager.Play("lampOnSound");
    }

    public void Reinitialize()
    {
        TurnOffLamp();
    }

    public void OnTilesMoved(Field field)
    {
        if (field.IsColumnOfType(columnNumber, lampType)) {
            if (!IsTurnedOn) TurnOnLamp();
        }
        else if (IsTurnedOn) TurnOffLamp();
    }
}
