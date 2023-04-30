using Firebase.Firestore;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Auction : MonoBehaviour
{
    [SerializeField]
    List<AuctionListing> auctionListings = new List<AuctionListing>();
    [SerializeField]
    GameObject auctionListingPanel;
    [SerializeField]
    AuctionListing auctionListingPrefab;
    [SerializeField]
    RegisterItemWindow registerItemWindow;

    private ListenerRegistration auctionListenerRegistration;


    public void Start()
    {
        registerItemWindow.InitRegisterItemWindow();
        InitAuctionListings();
    }

    public async void InitAuctionListings()
    {
        List<DocumentReference> auctionListingItemsRef = await FireBaseManager.instance.GetAuctionListingItemsRef();

        foreach (DocumentReference document in auctionListingItemsRef)
        {
            if (document == null)
            {
                continue;
            }

            FireBaseManager.instance.LoadAuctionListingItem(document, NewListing);
        }
        //StartAuctionListingsListener();
    }

    //fix listner not working correctly
    public void StartAuctionListingsListener()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference auctionsRef = db.Collection("auctions");

        auctionListenerRegistration = auctionsRef.Listen(snapshot =>
        {
            if (snapshot.Metadata.HasPendingWrites)
            {
                // Local changes have not been committed to the server yet
                return;
            }

            foreach (DocumentChange change in snapshot.GetChanges())
            {
                if (change.Document.Metadata.HasPendingWrites)
                {
                    // Local changes have not been committed to the server yet
                    continue;
                }
                switch (change.ChangeType)
                {
                    case DocumentChange.Type.Added:
                        FireBaseManager.instance.LoadAuctionListingItem(change.Document.Reference, NewListing);
                        break;
                    case DocumentChange.Type.Modified:
                        AuctionListingItem modifiedItem = change.Document.ConvertTo<AuctionListingItem>();
                        UpdateListing(modifiedItem);
                        break;
                    case DocumentChange.Type.Removed:
                        AuctionListingItem removedItem = change.Document.ConvertTo<AuctionListingItem>();
                        RemoveListing(removedItem);
                        break;
                }
            }
        });
    }

    private void UpdateListing(AuctionListingItem modifiedItem)
    {
        AuctionListing modifiedListing = auctionListings.Find(listing => listing.ListingId == modifiedItem.ListingId);
        if(modifiedListing == null)
        {
            Debug.Log("An Item Listed In DataBase was not found in Auction Listings");
            return;
        }

        modifiedListing.UpdateListing(modifiedItem);
    }

    public void NewListing(AuctionListingItem auctionListingItem)
    {
        AuctionListing auctionListing = Instantiate(auctionListingPrefab, auctionListingPanel.gameObject.transform);
        auctionListing.SetListing(auctionListingItem);

        auctionListings.Add(auctionListing);
    }

    public void RemoveListing(AuctionListingItem removedItem)
    {
        // Assuming AuctionListing has a property AuctionListingItem
        AuctionListing listingToRemove = null;

        foreach (AuctionListing listing in auctionListings)
        {
            if (listing.ListingId == removedItem.ListingId)
            {
                listingToRemove = listing;
                break;
            }
        }

        if (listingToRemove != null)
        {
            auctionListings.Remove(listingToRemove);
            Destroy(listingToRemove.gameObject);
        }
        else
        {
            Debug.LogWarning("Could not find the listing to remove");
        }
    }



    public void RegisterNewItem()
    {
        registerItemWindow.RegisterNewItemLogic(NewListing);
    }

    private void OnDestroy()
    {
        if (auctionListenerRegistration != null)
        {
            auctionListenerRegistration.Stop();
        }
    }
}
