using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

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


    public void SelectCharcter(int index){

    }

    public void DeleteCharcter(int index){

    }

    public void BeginScene()
    {
        GameManager.instance.ChangeScene("Scene_Forest_Town");
    }

    public void SignOut()
    {
        FireBaseManager.instance.SignOut();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
