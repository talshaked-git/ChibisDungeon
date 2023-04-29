using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonController : MonoBehaviour
{
    public Button[] buttons; // Assign the buttons in the Unity Inspector
    public Sprite[] defaultSprite;
    public Sprite[] activeSprite;
    private int activeButtonIndex = -1;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));
        }
    }

    public void OnButtonClick(int index)
    {
        if (index == activeButtonIndex)
        {
            // If the button is already active, deactivate it
            buttons[index].image.sprite = defaultSprite[index];
            activeButtonIndex = -1;
        }
        else
        {
            SetActiveButton(index);
        }
    }

    public void SetActiveButton(int index)
    {
        // If there's a previously active button, reset its sprite
        if (activeButtonIndex >= 0 && activeButtonIndex < buttons.Length)
        {
            buttons[activeButtonIndex].image.sprite = defaultSprite[activeButtonIndex];
        }

        // Update the active button index and set the new sprite
        activeButtonIndex = index;
        buttons[activeButtonIndex].image.sprite = activeSprite[activeButtonIndex];
    }
}
