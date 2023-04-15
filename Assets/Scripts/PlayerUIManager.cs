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

    [Header("Menu Panels")]
    [SerializeField]
    public GameObject MenuWindow;
    [SerializeField]
    public GameObject settingsMenu;
    [SerializeField]
    public GameObject exitMenu;
    public GameObject AuctionMenu;
    public GameObject ListItemWindow;
    public GameObject statsMenu;
    public GameObject inventoryPanel;
    public GameObject shopMenu;
    [SerializeField] ItemTooltip tooltip;

    private bool gamePaused = false;
    private bool isListItemWindowActive = false;

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
            statsMenu.SetActive(true);
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
            inventoryPanel.SetActive(true);
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
            AuctionMenu.SetActive(true);
            //settingsMenu.SetActive(true);
            //characterSwitchMenu.SetActive(true);
            //exitMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ShowRegisterItemToAuctionWindow()
    {
        ListItemWindow.SetActive(true);
        isListItemWindowActive = true;
        ShowInventoryFromStats();//this is a hack to show the inventory panel
    }

    public void HideAuction()
    {
        AuctionMenu.SetActive(false);
        if (isListItemWindowActive)
        {
            ListItemWindow.SetActive(false);
            isListItemWindowActive = false;
        }
        ResumeGame();
        //settingsMenu.SetActive(false);
        //characterSwitchMenu.SetActive(false);
        //exitMenu.SetActive(false);
    }

    

    void CloseAllMenus()
    {
        MenuWindow.SetActive(false);
        //settingsMenu.SetActive(false);
        //characterSwitchMenu.SetActive(false);
        //exitMenu.SetActive(false);
        AuctionMenu.SetActive(false);
        ListItemWindow.SetActive(false);
        statsMenu.SetActive(false);
        inventoryPanel.SetActive(false);
        ListItemWindow.SetActive(false);
        tooltip.HideTooltip();
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

    public void ShowInventoryFromStats(){
        inventoryPanel.SetActive(true);
    }
}
