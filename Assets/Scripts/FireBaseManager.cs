using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager instance;

    [Header("FireBase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    [Space(5f)]

    [Header("Login Refrences")]
    [SerializeField]
    private TMP_InputField emailInput;
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    public GameObject loginOutputUI;
    [SerializeField]
    public TMP_Text loginOutputText;
    [Space(5f)]

    [Header("Register Refrences")]
    [SerializeField]
    private TMP_InputField registerEmailInput;
    [SerializeField]
    private TMP_InputField registerPasswordInput;
    [SerializeField]
    private TMP_InputField registerConfirmPasswordInput;
    [SerializeField]
    private GameObject registerOutputUI;
    [SerializeField]
    private TMP_Text registerOutputText;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        StartCoroutine(CheckAndFixDependencies());
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var CheckAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(predicate: () => CheckAndFixDependenciesTask.IsCompleted);
        var dependancyResult = CheckAndFixDependenciesTask.Result;
        if (dependancyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependancyResult);
        }
    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if (user != null)
        {
            var reloadTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadTask.IsCompleted);
            AutoLogin();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            //TODO: add sign out then return autologin
            // GameManager.instance.ChangeScene("Scene_MainMenu");
            AuthUIManager.instance.LoginScreen();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    public void ClearOutputs()
    {
        loginOutputUI.SetActive(false);
        registerOutputUI.SetActive(false);
        loginOutputText.text = "";
        registerOutputText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(emailInput.text, passwordInput.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(RegisterLogic(registerEmailInput.text, registerPasswordInput.text, registerConfirmPasswordInput.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception == null)
        {
            if (!user.IsEmailVerified)
            {
                loginOutputUI.SetActive(true);
                loginOutputText.text = "Please Verify Your Email";
                yield break;
            }
            GameManager.instance.ChangeScene("Scene_MainMenu");
            yield break;
        }

        FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
        AuthError error = (AuthError)firebaseException.ErrorCode;

        string output = "";

        switch (error)
        {
            case AuthError.MissingEmail:
                output = "Yoho You Forgot Your Mail";
                break;
            case AuthError.MissingPassword:
                loginOutputText.text = "Yoho You Forgot Your Password";
                break;
            case AuthError.WrongPassword:
                output = "";
                break;
            case AuthError.InvalidEmail:
                output = "Invalid Email";
                break;
            case AuthError.UserNotFound:
                output = "Did You Register?";
                break;
            default:
                output = "Unknown Error";
                break;
        }

        loginOutputUI.SetActive(true);

        if (output == "")
        {
            loginOutputText.text = "LOL Did you even try?";
        }
        else
        {
            loginOutputText.text = output;
        }

    }

    private IEnumerator RegisterLogic(string _email, string _password, string _confirmPassword)
    {
        if (_email == "" || _password == "" || _confirmPassword == "")
        {
            RegisterOutput("You Forgot To Fill Something");
            yield break;

        }
        else if (_password != _confirmPassword)
        {
            RegisterOutput("Passwords Don't Match");
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception == null)
        {
            RegisterOutput("Registration Succesfull");
            yield return new WaitForSecondsRealtime(4);
            StartCoroutine(SendVerificationEmail());
            yield break;
        }

        FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
        AuthError error = (AuthError)firebaseException.ErrorCode;

        string output = "";

        switch (error)
        {
            case AuthError.WeakPassword:
                output = "Password Is Too Weak";
                break;
            case AuthError.EmailAlreadyInUse:
                output = "Looks Like Someone Is Already Useing This Email";
                break;
            case AuthError.InvalidEmail:
                output = "Mabey You Typed Your Email Wrong";
                break;
            default:
                output = "Unknown Error";
                break;
        }

        RegisterOutput(output);

    }

    private void RegisterOutput(string _output)
    {
        registerOutputUI.SetActive(true);
        registerOutputText.text = _output;
    }

    private void LoginOutput(string _output)
    {
        loginOutputUI.SetActive(true);
        loginOutputText.text = _output;
    }

    private IEnumerator SendVerificationEmail()
    {
        if (user == null)
            yield break;

        var emailTask = user.SendEmailVerificationAsync();

        yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

        if (emailTask.Exception == null)
        {
            AuthUIManager.instance.AwaitVerification(true, user.Email, "Verification Email Was Sent!");
            Debug.Log("Verification Email Sent Succesfully");
            yield break;
        }


        FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
        AuthError error = (AuthError)firebaseException.ErrorCode;

        string output = "";

        switch (error)
        {
            case AuthError.Cancelled:
                output = "Verification Task Cancelled";
                break;
            case AuthError.InvalidRecipientEmail:
                loginOutputText.text = "Invalid Email";
                break;
            case AuthError.TooManyRequests:
                output = "Too Many Requests";
                break;
            default:
                output = "Unknown Error";
                break;
        }

        AuthUIManager.instance.AwaitVerification(false, user.Email, output);

    }

    public void SignOut()
    {
        auth.SignOut();
        GameManager.instance.ChangeScene("Scene_LoginRegister");
    }

}