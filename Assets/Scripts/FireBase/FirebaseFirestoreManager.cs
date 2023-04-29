using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using System.Collections.Generic;
using System;

public class FirebaseFirestoreManager : MonoBehaviour
{
    public FirebaseFirestore db;

    public void Initialize()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task LoadAccount(Action<Account> callback)
    {
        FirebaseUser user = FireBaseManager.instance.firebaseAuthManager.user;

        if (user == null)
        {
            Debug.LogError("User is not logged in!");
            return;
        }

        string userId = user.UserId;

        DocumentReference docRef = db.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Account account = snapshot.ConvertTo<Account>();
            callback(account);
        }
        else
        {
            Debug.LogError("User document does not exist!");
        }
    }

    public async Task SaveAccount(Account account)
    {
        FirebaseUser user = FireBaseManager.instance.firebaseAuthManager.user;

        if (user == null)
        {
            Debug.LogError("User is not logged in!");
            return;
        }

        string userId = user.UserId;

        DocumentReference docRef = db.Collection("users").Document(userId);

        //convert account using firestoreData
        
        await docRef.SetAsync(account, SetOptions.MergeAll);
    }

    public DocumentReference SaveNewPlayer(Player player)
    {
        DocumentReference docRef = db.Collection("players").Document(player.CID);
        docRef.SetAsync(player);
        return docRef;
    }

    public DocumentReference GetAccountRef(Account account)
    {
        FirebaseUser user = FireBaseManager.instance.firebaseAuthManager.user;
        if (user == null)
        {
            Debug.LogError("User is not logged in!");
            return null;
        }
        string userId = user.UserId;
        DocumentReference docRef = db.Collection("users").Document(userId);
        return docRef;
    }

    public async Task LoadPlayer(string cid, Action<Player> callback)
    {
        DocumentReference docRef = db.Collection("players").Document(cid);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Player player = snapshot.ConvertTo<Player>();
            callback(player);
        }
        else
        {
            Debug.LogError("Player document does not exist!");
        }
    }

    public void SavePlayer(Player player)
    {
        DocumentReference docRef = db.Collection("players").Document(player.CID);
        docRef.SetAsync(player, SetOptions.MergeAll);
    }
}
