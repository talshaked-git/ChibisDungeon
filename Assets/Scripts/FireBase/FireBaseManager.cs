using Firebase.Firestore;
using System;
using System.Collections.Generic;
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
            return;
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

    public Task<Account> LoadAccount()
    {
        return firebaseFirestoreManager.LoadAccount();
    }

    public async void LoadPlayer(string playerId, Action<Player> callback)
    {
        await firebaseFirestoreManager.LoadPlayer(playerId, callback);
    }

    public void SavePlayer (Player player)
    {
        firebaseFirestoreManager.SavePlayer(player);
    }

    public Task RegisterItemToAuction(AuctionListingItem auctionListingItem, Player player)
    {
         return firebaseFirestoreManager.RegisterItemToAuction(auctionListingItem, player);
    }

    public Task<List<DocumentReference>> GetAuctionListingItemsRef()
    {
        return firebaseFirestoreManager.GetAuctionListingItemsRef();
    }

    internal void LoadAuctionListingItem(DocumentReference document, Action<AuctionListingItem> newListing)
    {
        firebaseFirestoreManager.LoadAuctionListingItem(document, newListing);
    }

    public Task<AuctionListingItem> GetAuctionListingItem(string listingItemId)
    {
        return firebaseFirestoreManager.GetAuctionListingItem(listingItemId);
    }

    public Task UpdateAuctionListingBid(AuctionListingItem auctionListingItem, Player player)
    {
        return firebaseFirestoreManager.UpdateAuctionListingBid(auctionListingItem,player);
    }

    public Task RemoveAuctionListing(string listingId, Player player)
    {
        return firebaseFirestoreManager.RemoveAuctionListing(listingId, player);
    }

    public void LoginButton()
    {
        firebaseAuthManager.LoginButton();
    }

    public void RegisterButton()
    {
        firebaseAuthManager.RegisterButton();
    }

    public void SetAuthUIManager(AuthUIManager authUIManager)
    {
        firebaseAuthManager.SetAuthUIManager(authUIManager);
    }
}
