using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    [Header("Refrences")]
    [SerializeField]
    private GameObject SettingUI;
    [SerializeField]
    private GameObject CharcterAddUI;
    [SerializeField]
    private GameObject AboutUI;



    private bool isSetttingUION = false;
    private bool isAboutUION = false;
    private bool isCharcterAddUION = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void ClearUI()
    {
        SettingUI.SetActive(false);
        CharcterAddUI.SetActive(false);
        AboutUI.SetActive(false);
        //TODO: Clear all UI
    }


    public void SettingScreen()
    {
        ClearUI();
        isAboutUION = false;
        isCharcterAddUION = false;
        isSetttingUION = !isSetttingUION;
        SettingUI.SetActive(isSetttingUION);
    }

    public void CharcterAddScreen()
    {
        ClearUI();
        isSetttingUION = false;
        isAboutUION = false;
        isCharcterAddUION = !isCharcterAddUION;
        CharcterAddUI.SetActive(isCharcterAddUION);
    }

    public void AboutScreen()
    {
        ClearUI();
        isSetttingUION = false;
        isCharcterAddUION = false;
        isAboutUION = !isAboutUION;
        AboutUI.SetActive(isAboutUION);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

}
