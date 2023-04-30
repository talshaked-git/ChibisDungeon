using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("Menu Buttons")]
    public Button menuButton;
    public Button statsButton;
    public Button inventoryButton;
    public Button shopButton;
    public Button auctionButton;

    [Header("Menu Panels")]
    [SerializeField]
    public GameObject MenuWindow;
    [SerializeField]
    public GameObject settingsMenu;
    [SerializeField]
    public GameObject exitMenu;
    // public GameObject characterSwitchMenu;
    public GameObject statsMenu;
    public GameObject inventoryPanel;
    public GameObject shopMenu;
    public GameObject auctionMenu;
    [SerializeField] 
    public GameObject RegisterItemPanel;
    [SerializeField] ItemTooltip tooltip;

    private bool gamePaused = false;

    //create awake method and dont destroy on scene change
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CloseAllMenus();
    }

    public void ShowMenu()
    {
        if (!gamePaused)
        {
            PauseGame();
            DropItemAreaShow();
            MenuWindow.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }

        //settingsMenu.SetActive(true);
        //characterSwitchMenu.SetActive(true);
        //exitMenu.SetActive(true);
    }

    public void ShowStats()
    {
        if (!gamePaused)
        {
            PauseGame();
            DropItemAreaShow();
            statsMenu.SetActive(true);
            PlayerManager.instance.UpdateStatusPanel();
            //settingsMenu.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ShowInventory()
    {
        if (!gamePaused)
        {
            PauseGame();
            DropItemAreaShow();
            inventoryPanel.SetActive(true);
            inventoryPanel.GetComponentInChildren<Scrollbar>().value = 1;
        }
        else
        {
            ResumeGame();
        }
    }

    public void ShowShop()
    {
        if (!gamePaused)
        {
            PauseGame();
            //settingsMenu.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ShowAuction()
    {
        if (!gamePaused)
        {
            PauseGame();
            auctionMenu.SetActive(true);
            //settingsMenu.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ShowAuctionAddItemPanel()
    {
            RegisterItemPanel.SetActive(true);
            //settingsMenu.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
    }

    void CloseAllMenus()
    {
        MenuWindow.SetActive(false);
        //settingsMenu.SetActive(false);
        //characterSwitchMenu.SetActive(false);
        //exitMenu.SetActive(false);
        statsMenu.SetActive(false);
        inventoryPanel.SetActive(false);
        tooltip.HideTooltip();
        DropItemAreaHide();
        PlayerManager.isTooltipActive = false;
        //shopMenu.SetActive(false);
    }

    void PauseGame()
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
            CloseAllMenus();
        }
    }

    public void ShowInventoryFromStats()
    {
        inventoryPanel.SetActive(true);
    }

    public void ScrollUp()
    {
        inventoryPanel.GetComponentInChildren<Scrollbar>().value += 0.1f;
    }

    public void ScrollDown()
    {
        inventoryPanel.GetComponentInChildren<Scrollbar>().value -= 0.1f;
    }

    public void DropItemAreaShow()
    {
        PlayerManager.instance.ShowDropItemArea();
    }

    public void DropItemAreaHide()
    {
        PlayerManager.instance.HideDropItemArea();
    }

    public void ShowAttributeAllocation()
    {
        PlayerManager.instance.ShowAttributeAllocationPanel();
    }
}
