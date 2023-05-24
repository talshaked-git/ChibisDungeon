using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Google;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAuthenticationManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;

    public string GoogleWebAPI = "844846155287-7n7i8ehjeci2r4mfpjdk1us5ormjpeqj.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    AuthUIManager authUIManager;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;


    public void SetAuthUIManager(AuthUIManager authUIManager)
    {
        this.authUIManager = authUIManager;
    }

    public void Start()
    {
        authUIManager = FindObjectOfType<AuthUIManager>();
    }

    public void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        InitConfiguration();
        StartCoroutine(CheckAutoLogin());
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
                StartCoroutine(CreateNewAccountDocument());
                //if (user.IsEmailVerified)
                //{
                //  StartCoroutine(CreateNewAccountDocument());
                //}
            }
        }
    }

    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }

    private IEnumerator CreateNewAccountDocument()
    {
        string userId = user.UserId;
        yield return new WaitUntil(predicate: () => FireBaseManager.instance.firebaseFirestoreManager.isInitialized);
        FireBaseManager.instance.CreateNewAccountDocument(userId);
    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(predicate: () => FireBaseManager.instance.firebaseFirestoreManager.isInitialized);
        if (user != null)
        {
            var reloadTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadTask.IsCompleted);
            AutoLogin();
        }
        else
        {

            authUIManager.LoginScreen();
        }
    }


    private async void AutoLogin()
    {
        if (user != null)
        {
            Task<Account> taskAccount = FireBaseManager.instance.LoadAccount();
            await taskAccount;
            if (taskAccount == null || taskAccount.IsCompletedSuccessfully)
            {
                if(taskAccount == null)
                {
                    authUIManager.LoginOutputUIShow("Error Loading Account During AutoLogin");
                    Debug.Log("Error Loading Account During AutoLogin ACcount = NULL");
                    return;
                }
                Account account = taskAccount.Result;
                GameManager.instance.account = account;
                GameManager.instance.ChangeScene("Scene_MainMenu");
            }
            else
            {
                authUIManager.LoginOutputUIShow( "Error Loading Account During AutoLogin");
            }
        }
        else
        {
            authUIManager.LoginScreen();
        }
    }

    public void LoginButton()
    {
        authUIManager.SavePrefs();
        string email = authUIManager.emailInput.text;
        string password = authUIManager.passwordInput.text;

        LoginLogic(email,password);
    }

    public void RegisterButton()
    {
        string email = authUIManager.registerEmailInput.text;
        string password = authUIManager.registerPasswordInput.text;
        string confirmPassword = authUIManager.registerConfirmPasswordInput.text;
        StartCoroutine(RegisterLogic(email,password,confirmPassword));
    }

    private async void LoginLogic(string _email, string _password)
    {
        if (auth == null)
        {
            Debug.LogError("Auth is null");
            return;
        }

        if (authUIManager == null)
        {
            Debug.LogError("authUIManager is null");
            return;
        }

        if (FireBaseManager.instance == null)
        {
            Debug.LogError("FireBaseManager.instance is null");
            return;
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance is null");
            return;
        }

        try
        {
            Credential credential = EmailAuthProvider.GetCredential(_email, _password);
            var loginTask = await auth.SignInWithCredentialAsync(credential);

            var user = loginTask;
            if (user == null)
            {
                Debug.LogError("User is null");
                return;
            }

            if (!user.IsEmailVerified)
            {
                authUIManager.LoginOutputUIShow("Please Verify Your Email");
                Debug.Log("Please Verify Your Email");
                return;
            }

            Task<Account> taskAccount = FireBaseManager.instance.LoadAccount();
            Account account = await taskAccount;

            if (taskAccount.IsFaulted)
            {
                authUIManager.LoginOutputUIShow("Error Loading Account");
                Debug.Log($"Error Loading Account. Error: {taskAccount.Exception}");
                return;
            }

            GameManager.instance.account = account;
            Debug.Log("Login Succesfull");
            GameManager.instance.ChangeScene("Scene_MainMenu");
        }
        catch (Exception ex)
        {
            if (ex is FirebaseException firebaseException)
            {
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "";

                switch (error)
                {
                    case AuthError.MissingEmail:
                        output = "Yoho You Forgot Your Mail";
                        break;
                    case AuthError.MissingPassword:
                        output = "Yoho You Forgot Your Password";
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

                if (output == "")
                {
                    output = "LOL Did you even try?";
                }
                authUIManager.LoginOutputUIShow(output);
            }
            else
            {
                Debug.LogError($"Exception occurred: {ex}");
            }
        }
    }



    //private async void LoginLogic(string _email, string _password)
    //{
    //    Credential credential = EmailAuthProvider.GetCredential(_email, _password);


    //        var loginTask = auth.SignInWithCredentialAsync(credential);
    //        await loginTask.ContinueWithOnMainThread(async task =>
    //        {
    //            if (task.IsFaulted)
    //            {
    //                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
    //                if (firebaseException != null)
    //                {
    //                    AuthError error = (AuthError)firebaseException.ErrorCode;

    //                    string output = "";

    //                    switch (error)
    //                    {
    //                        case AuthError.MissingEmail:
    //                            output = "Yoho You Forgot Your Mail";
    //                            break;
    //                        case AuthError.MissingPassword:
    //                            output = "Yoho You Forgot Your Password";
    //                            break;
    //                        case AuthError.WrongPassword:
    //                            output = "";
    //                            break;
    //                        case AuthError.InvalidEmail:
    //                            output = "Invalid Email";
    //                            break;
    //                        case AuthError.UserNotFound:
    //                            output = "Did You Register?";
    //                            break;
    //                        default:
    //                            output = "Unknown Error";
    //                            break;
    //                    }

    //                    if (output == "")
    //                    {
    //                        output = "LOL Did you even try?";
    //                    }
    //                    authUIManager.LoginOutputUIShow(output);
    //                    return;
    //                }
    //            }

    //            //if (!user.IsEmailVerified)
    //            //{
    //            //    authUIManager.LoginOutputUIShow("Please Verify Your Email");
    //            //    Debug.Log("Please Verify Your Email");
    //            //    return;
    //            //}

    //            Task<Account> taskAccount = FireBaseManager.instance.LoadAccount();
    //            await taskAccount;
    //            if (taskAccount.IsCompletedSuccessfully)
    //            {
    //                Account account = taskAccount.Result;
    //                GameManager.instance.account = account;
    //                Debug.Log("Login Succesfull");
    //                GameManager.instance.ChangeScene("Scene_MainMenu");
    //            }
    //            else
    //            {
    //                authUIManager.LoginOutputUIShow("Error Loading Account");
    //                Debug.Log("Error Loading Account");
    //            }
    //        });
    //}

    private IEnumerator RegisterLogic(string _email, string _password, string _confirmPassword)
    {
        if (_email == "" || _password == "" || _confirmPassword == "")
        {
            authUIManager.RegisterOutput("You Forgot To Fill Something");
            yield break;

        }
        else if (_password != _confirmPassword)
        {
            authUIManager.RegisterOutput("Passwords Don't Match");
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception == null)
        {
            authUIManager.RegisterOutput("Registration Succesfull");
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

        authUIManager.RegisterOutput(output);

    }

    private IEnumerator SendVerificationEmail()
    {
        if (user == null)
            yield break;

        var emailTask = user.SendEmailVerificationAsync();

        yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

        if (emailTask.Exception == null)
        {
            authUIManager.AwaitVerification(true, user.Email, "Verification Email Was Sent!");
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
                output = "Invalid Email";
                break;
            case AuthError.TooManyRequests:
                output = "Too Many Requests";
                break;
            default:
                output = "Unknown Error";
                break;
        }

        authUIManager.AwaitVerification(false, user.Email, output);
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
