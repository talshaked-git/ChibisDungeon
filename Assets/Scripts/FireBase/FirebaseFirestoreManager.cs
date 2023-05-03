using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using System.Collections.Generic;
using System;
using Firebase;

public class FirebaseFirestoreManager : MonoBehaviour
{
    public FirebaseFirestore db;
    public bool isInitialized = false;
    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    WriteBatch batch;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                isInitialized = true;
                Debug.Log("FirebaseFirestoreManager: Connected to Firebase!");
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public Task<Account> LoadAccount()
    {
        FirebaseUser user = FireBaseManager.instance.firebaseAuthManager.user;

        if (user == null)
        {
            Debug.LogError("User is not logged in!");
            return null;
        }

        string userId = user.UserId;

        DocumentReference docRef = db.Collection("users").Document(userId);
        if (docRef == null)
        {
            CreateNewAccountDocument(userId);
        }

        return docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Account account = snapshot.ConvertTo<Account>();
                return account;
            }
            else
            {
                Debug.Log("Account document does not exist!");
                Account account = new Account(userId);
                db.Collection("users").Document(userId).SetAsync(account);
                return account;
            }
        });

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

    public Task RegisterItemToAuction(AuctionListingItem auctionListingItem,Player player)
    {
        FirebaseUser user = FireBaseManager.instance.firebaseAuthManager.user;
        if (user == null)
        {
            Debug.LogError("User is not logged in!");
            return null;
        }
        DocumentReference docRef = db.Collection("auctions").Document();
        batch = db.StartBatch();
        batch.Set(docRef, auctionListingItem);

        DocumentReference playerRef = db.Collection("players").Document(player.CID);
        batch.Set(playerRef, player,SetOptions.MergeAll);
        
        return batch.CommitAsync();
    }

    public async Task<List<DocumentReference>> GetAuctionListingItemsRef()
    {
        List<DocumentReference > items = new List<DocumentReference>();
        QuerySnapshot querySnapshot = await db.Collection("auctions").GetSnapshotAsync();

        List<DocumentReference> documentReferences = new List<DocumentReference>();

        foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
        {
            documentReferences.Add(documentSnapshot.Reference);
        }

        return documentReferences;
    }

    public async void LoadAuctionListingItem(DocumentReference documentReference, Action<AuctionListingItem> callback)
    {
        DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            AuctionListingItem auctionListingItem = snapshot.ConvertTo<AuctionListingItem>();
            callback(auctionListingItem);
        }
        else
        {
            Debug.LogError("AuctionListingItem document does not exist!");
        }
    }

    public async Task<AuctionListingItem> GetAuctionListingItem(string listingId)
    {
        DocumentReference docRef = db.Collection("auctions").Document(listingId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            AuctionListingItem auctionListingItem = snapshot.ConvertTo<AuctionListingItem>();
            Debug.Log(auctionListingItem);
            return auctionListingItem;
        }
        else
        {
            Debug.LogError("AuctionListingItem document does not exist!");
            return null;
        }
    }

    public Task UpdateAuctionListingBid(AuctionListingItem auctionListingItem, Player player)
    {
        batch = db.StartBatch();
        DocumentReference docRef = db.Collection("auctions").Document(auctionListingItem.ListingId);
        DocumentReference docRef2 = db.Collection("players").Document(player.CID);


        batch.Set(docRef2, player, SetOptions.MergeAll);
        batch.Set(docRef,auctionListingItem, SetOptions.MergeAll);

        return batch.CommitAsync();
    }

    public Task RemoveAuctionListing(string listingId,Player player)
    {
        batch = db.StartBatch();
        DocumentReference docRef = db.Collection("auctions").Document(listingId);
        DocumentReference docRef2 = db.Collection("players").Document(player.CID);
        batch.Delete(docRef);
        batch.Set(docRef2, player, SetOptions.MergeAll);
        return batch.CommitAsync();
    }

    public async void CreateNewAccountDocument(string userId)
    {
        if(db == null)
        {
            Debug.LogError("Database is null!");
            return;
        }
        DocumentReference accounRef = db.Collection("users").Document(userId);
        var accountSnapshot = accounRef.GetSnapshotAsync();
        await accountSnapshot;
        if (accountSnapshot.Result.Exists)
        {
            Debug.Log("Account document already exists!");
            return;
        }
        else
        {
            Account account = new Account(userId);
            Task createAccountTask = accounRef.SetAsync(account);
            await createAccountTask;
            if (createAccountTask.IsCompletedSuccessfully)
            {
                Debug.Log("Account document created successfully!");
                GameManager.instance.account = account;
            }
            else
            {
                Debug.LogError("Account document creation failed!");
            }
        }
        
    }

}
