using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager instance;

    [Header("Refrences")]
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    private Toggle rememberMe;




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

    private void Start()
    {
        LoadPrefs();
    }

    private void ClearUI()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        FireBaseManager.instance.firebaseAuthManager.ClearOutputs();
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
        FireBaseManager.instance.firebaseAuthManager.loginOutputUI.SetActive(true);

        if (_emailSent)
        {
            FireBaseManager.instance.firebaseAuthManager.loginOutputText.text = _output + _email;
        }
        else
        {
            FireBaseManager.instance.firebaseAuthManager.loginOutputText.text = _output;
        }
    }

    public void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("email"))
        {
            FireBaseManager.instance.firebaseAuthManager.emailInput.text = PlayerPrefs.GetString("email");
            rememberMe.isOn = true;
        }
    }

    public void SavePrefs(string _email)
    {
        if (rememberMe.isOn)
        {
            PlayerPrefs.SetString("email", _email);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("email");
            PlayerPrefs.Save();
        }
    }

}
