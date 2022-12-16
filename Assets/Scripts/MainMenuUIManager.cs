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


    private bool issSetttingUION = false;


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
        //TODO: Clear all UI
    }

    public void SettingScreen()
    {
        ClearUI();
        issSetttingUION = !issSetttingUION;
        SettingUI.SetActive(issSetttingUION);
    }

    public void CharcterAddScreen()
    {
        ClearUI();
        CharcterAddUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
