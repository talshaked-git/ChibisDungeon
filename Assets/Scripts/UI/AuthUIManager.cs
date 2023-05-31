using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    private Toggle rememberMe;

    [Header("Login Refrences")]
    [SerializeField]
    public TMP_InputField emailInput;
    [SerializeField]
    public TMP_InputField passwordInput;
    [SerializeField]
    public GameObject loginOutputUI;
    [SerializeField]
    public TMP_Text loginOutputText;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button registerUIButton;
    [Space(5f)]

    [Header("Register Refrences")]
    [SerializeField]
    public TMP_InputField registerEmailInput;
    [SerializeField]
    public TMP_InputField registerPasswordInput;
    [SerializeField]
    public TMP_InputField registerConfirmPasswordInput;
    [SerializeField]
    public GameObject registerOutputUI;
    [SerializeField]
    public TMP_Text registerOutputText;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Button loginUIButton;


    private void Start()
    {
        LoadPrefs();
        InitButtons();
        FireBaseManager.instance.SetAuthUIManager(this);
    }

    private void InitButtons()
    {
        loginButton.onClick.AddListener(FireBaseManager.instance.LoginButton);
        registerButton.onClick.AddListener(FireBaseManager.instance.RegisterButton);
        loginUIButton.onClick.AddListener(LoginScreen);
        registerUIButton.onClick.AddListener(RegisterScreen);
    }

    private void ClearUI()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        ClearOutputs();
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
        loginOutputUI.SetActive(true);

        if (_emailSent)
        {
            loginOutputText.text = _output + _email;
        }
        else
        {
            loginOutputText.text = _output;
        }
    }

    public void LoadPrefs()
    {
        try
        {
            if (PlayerPrefs.HasKey("email"))
            {
                emailInput.text = PlayerPrefs.GetString("email");
                rememberMe.isOn = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("error loading email");
        }

    }

    public void SavePrefs()
    {
        try
        {
            string email = emailInput.text;
            if (rememberMe.isOn)
            {
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.DeleteKey("email");
                PlayerPrefs.Save();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("error saving email");
        }

    }

    public void LoginOutputUIShow(string text)
    {
        loginOutputUI.SetActive(true);
        loginOutputText.text = text;
    }

    public void RegisterOutput(string _output)
    {
        registerOutputUI.SetActive(true);
        registerOutputText.text = _output;
    }

    public void LoginOutput(string _output)
    {
        loginOutputUI.SetActive(true);
        loginOutputText.text = _output;
    }

    public void ClearOutputs()
    {
        loginOutputUI.SetActive(false);
        registerOutputUI.SetActive(false);
        loginOutputText.text = "";
        registerOutputText.text = "";
    }

}
