using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FirebaseAuthenticationManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;

    public string GoogleWebAPI = "844846155287-7n7i8ehjeci2r4mfpjdk1us5ormjpeqj.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    // Your UI components and other variables can be added here
    [Header("Login Refrences")]
    [SerializeField]
    public TMP_InputField emailInput;
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

    public void Initialize()
    {
        StartCoroutine(CheckAndFixDependencies());
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        if (checkAndFixDependenciesTask.Result == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            InitConfiguration();
            StartCoroutine(CheckAutoLogin());
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + checkAndFixDependenciesTask.Result);
        }
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
                if (user.IsEmailVerified)
                {
                    StartCoroutine(CreateNewAccountDocument());
                }
            }
        }
    }

    private IEnumerator CreateNewAccountDocument()
    {
        string userId = user.UserId;
        DocumentReference docRef = FireBaseManager.instance.firebaseFirestoreManager.db.Collection("users").Document(userId);

        var getDocTask = docRef.GetSnapshotAsync();
        yield return new WaitUntil(predicate: () => getDocTask.IsCompleted);

        if (getDocTask.Result.Exists)
        {
            Debug.Log("User document already exists.");
        }
        else
        {
            Debug.Log("Creating new user document.");

            Account newAccount = new Account(userId);

            var createAccountTask = docRef.SetAsync(newAccount);
            yield return new WaitUntil(predicate: () => createAccountTask.IsCompleted);

            if (createAccountTask.Exception == null)
            {
                Debug.Log("New user document created successfully.");
            }
            else
            {
                Debug.LogError("Error creating account document: " + createAccountTask.Exception.Message);
            }
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
            GameManager.instance.LoadAccount();
            GameManager.instance.ChangeScene("Scene_MainMenu");
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    public void LoginButton()
    {
        AuthUIManager.instance.SavePrefs(emailInput.text);
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
            GameManager.instance.LoadAccount();
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

    public void ClearOutputs()
    {
        loginOutputUI.SetActive(false);
        registerOutputUI.SetActive(false);
        loginOutputText.text = "";
        registerOutputText.text = "";
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

    private void InitConfiguration()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };
    }

    public void SignOut()
    {
        auth.SignOut();
        GameManager.instance.ChangeScene("Scene_LoginRegister");
    }

    public void GoogleLoginButton()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished);
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsCanceled)
        {
            Debug.LogError("SignInWithGoogle canceled");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("SignInWithGoogle encountered an error: " + task.Exception);
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            user = auth.CurrentUser;
            GameManager.instance.ChangeScene("Scene_MainMenu");
        });


    }
}
