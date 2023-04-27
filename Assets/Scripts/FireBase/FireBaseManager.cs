using Firebase.Firestore;
using UnityEngine;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager instance;

    [Header("Firebase Managers")]
    public FirebaseAuthenticationManager firebaseAuthManager;
    public FirebaseFirestoreManager firebaseFirestoreManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        firebaseAuthManager.Initialize();
        firebaseFirestoreManager.Initialize();
    }

    public DocumentReference SaveNewPlayer(Player player)
    {
        return firebaseFirestoreManager.SaveNewPlayer(player);
    }

    public async void UpdateAccountData(Account account)
    {
        await firebaseFirestoreManager.SaveAccount(account);
    }
    

}
