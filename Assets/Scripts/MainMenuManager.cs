using System.Collections;
using System.Collections.Generic;
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

    public void BeginScene()
    {
        GameManager.instance.ChangeScene("Scene_Forest_Town");
    }

    public void SignOut()
    {
        FireBaseManager.instance.SignOut();
    }

}
