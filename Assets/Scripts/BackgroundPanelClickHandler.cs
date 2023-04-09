using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundPanelClickHandler : MonoBehaviour, IPointerClickHandler
{
    public PlayerUIManager uiManager;
    public GameObject menuWindow;
    public GameObject statsMenu;
    public GameObject inventoryMenu;
    public GameObject shopMenu;


    public void OnPointerClick(PointerEventData eventData)
    {
        menuWindow.SetActive(false);
        statsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        shopMenu.SetActive(false);
        
        uiManager.ResumeGame();
    }

}
