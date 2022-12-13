using TMPro;
using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager instance;

    [Header("Refrences")]
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;



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
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        FireBaseManager.instance.ClearOutputs();
    }

    public void LoginScreen()
    {
        ClearUI();
        loginUI.SetActive(true);
    }

    public void RegisterScreen()
    {
        ClearUI();
        registerUI.SetActive(true);
    }

    public void AwaitVerification(bool _emailSent, string _email, string _output)
    {
        LoginScreen();
        FireBaseManager.instance.loginOutputUI.SetActive(true);

        if (_emailSent)
        {
            FireBaseManager.instance.loginOutputText.text = _output + _email;
        }
        else
        {
            FireBaseManager.instance.loginOutputText.text = _output;
        }
    }

}
