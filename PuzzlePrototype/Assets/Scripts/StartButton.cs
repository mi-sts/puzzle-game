using UnityEngine.UI;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetActive()
    {
        button.interactable = true;
    }

    public void OnButtonPressed()
    {
        FieldController.controller.StartNewGame();
        button.interactable = false;
    }
}
