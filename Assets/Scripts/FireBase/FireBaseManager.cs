using Firebase.Firestore;
using System;
using System.Threading.Tasks;
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

    public async Task LoadAccount(Action<Account> callback)
    {
        await firebaseFirestoreManager.LoadAccount(callback);
    }

    public async void LoadPlayer(string playerId, Action<Player> callback)
    {
        await firebaseFirestoreManager.LoadPlayer(playerId, callback);
    }

    public void SavePlayer (Player player)
    {
        firebaseFirestoreManager.SavePlayer(player);
    }
}
