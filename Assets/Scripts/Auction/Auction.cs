using Firebase.Firestore;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

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
    [SerializeField]
    TMP_InputField myBid;
    [SerializeField]
    InformationPanel informationPanel;

    private ListenerRegistration auctionListenerRegistration;

    public AuctionListing selectedListing;

    Color UnselectedColor = Color.white;
    Color SelectedColor = new Color(0.5f, 0.5f, 0.5f, 1);

    private bool _isTooltipActive = false;

    public void Start()
    {
        registerItemWindow.InitRegisterItemWindow();
        InitAuctionListings();
    }

    public void InitAuctionListings()
    {
        StartAuctionListingsListener();
    }

    //fix listner not working correctly
    public void StartAuctionListingsListener()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        auctionListenerRegistration = db.Collection("auctions").OrderByDescending("ExpirationTime").Limit(30).Listen(snapshot =>
        {
            foreach (DocumentChange change in snapshot.GetChanges())
            {
                Debug.Log(change.ToString());
                Debug.Log("Change Type: " + change.ChangeType);
                if (change.ChangeType == DocumentChange.Type.Added)
                {
                    AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                    AuctionListing existingListing = auctionListings.Find(listing => listing.ListingId == auctionListingItem.ListingId);
                    if (existingListing == null)
                    {
                        NewListing(auctionListingItem);
                    }
                    else
                    {
                        UpdateListing(existingListing, auctionListingItem);
                    }
                }
                else if( change.ChangeType == DocumentChange.Type.Modified)
                {
                    AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                    AuctionListing existingListing = auctionListings.Find(listing => listing.ListingId == auctionListingItem.ListingId);
                    if (existingListing == null)
                    {
                        NewListing(auctionListingItem);
                    }
                    else
                    {
                        UpdateListing(existingListing, auctionListingItem);
                    }
                }
                else if(change.ChangeType == DocumentChange.Type.Removed)
                {
                    AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                    RemoveListing(auctionListingItem);
                }
                else
                {
                    Debug.Log("Unknown Document Change Type");
                }
            }
        });
    }

    private void UpdateListing(AuctionListing existingListing,AuctionListingItem modifiedItem)
    {

        existingListing.UpdateListing(modifiedItem);
    }

    public void NewListing(AuctionListingItem auctionListingItem)
    {
        AuctionListing auctionListing = Instantiate(auctionListingPrefab, auctionListingPanel.gameObject.transform);
        auctionListing.SetListing(auctionListingItem);
        auctionListing.OnPressEvent += ShowTooltip;
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

    public async void BuyOutItem()
    {
        if(selectedListing == null)
        {
            Debug.Log("No Listing Selected");
            ShowInfoPanel("No Listing Selected");
            return;
        }
        //Add to logic to check if player has enough money


        AuctionListingItem auctionListingItem = await FireBaseManager.instance.GetAuctionListingItem(selectedListing.ListingId);
        if (auctionListingItem == null)
        {
            Debug.Log("AuctionListingItem is null");
            return;
        }

        if (auctionListingItem.SellerId == PlayerManager.instance.CurrentPlayer.CID)
        {
            Debug.Log("Bids On your own Item");
            ShowInfoPanel("Can't Buy Out your own Item");
            return;
        }


        if (!PlayerManager.instance.RemoveGold(auctionListingItem.GetBuyoutPrice()))
        {
            ShowInfoPanel("Not Enough Gold\n To Buy Out Item");
            return;
        }

        PlayerManager.instance.AddItem(auctionListingItem.Item.ToItem());
        PlayerManager.instance.SavePlayerData();

        //Remove Listing
        Task buyOutTask =  FireBaseManager.instance.RemoveAuctionListing(auctionListingItem.ListingId,PlayerManager.instance.CurrentPlayer);
        await buyOutTask;
        if (buyOutTask.IsFaulted)
        {
            Debug.Log("Buy Out Failed");
            PlayerManager.instance.RemoveItem(auctionListingItem.Item.ToItem());
            PlayerManager.instance.SavePlayerData();
            ShowInfoPanel("Buy Out Failed");
        }
        else
        {
            Debug.Log("Buy Out Successful");
            
        }
    }

    public async void BidOnItem()
    {
        if(selectedListing == null)
        {
            Debug.Log("No Listing Selected");
            ShowInfoPanel("No Listing Selected");
            return;
        }

        //Add logic to check if bid is higher than current bid
        AuctionListingItem auctionListingItem = await FireBaseManager.instance.GetAuctionListingItem(selectedListing.ListingId);
        if(auctionListingItem == null)
        {
            Debug.Log("AuctionListingItem is null");
            return;
        }

        if (auctionListingItem.SellerId == PlayerManager.instance.CurrentPlayer.CID)
        {
            Debug.Log("Bids On your own Item");
            ShowInfoPanel("Can't Bid on your own Item");
            return;
        }

        int bid = int.Parse(myBid.text);
        int topBidder = int.Parse(auctionListingItem.TopBid);
        if(bid  <= topBidder)
        {
            Debug.Log("Bid is not higher than current bid");
            ShowInfoPanel("Bid is not higher than current bid");
            return;
        }

        //Add logic to check if bid is higher than buyout price
        int buyoutPrice = int.Parse(auctionListingItem.BuyoutPrice);
        if(bid >= buyoutPrice)
        {
            //Check if player has wants to buy out instead
            Debug.Log("Bid is higher than buyout price");
            ShowInfoPanel("Bid is higher than buyout price.\n Consider Buyout");
            return;
        }

        if (!PlayerManager.instance.RemoveGold(int.Parse(myBid.text)))
        {
            ShowInfoPanel("Not Enough Gold To Bid");
            return;
        }

        //Update Bid
        string previousBidderId = auctionListingItem.BidderId;
        string previousBid = auctionListingItem.TopBid;
        Dictionary<string,string> lastBidder =new Dictionary<string, string>{ { "Id", previousBidderId },{"Bid", previousBid }};
        string bidderId = PlayerManager.instance.CurrentPlayer.CID;

        if(auctionListingItem.Lastbider == null)
        {
            //add login to return money to previous bidder here or DB trigger
        }
        auctionListingItem.BidderId = bidderId;
        auctionListingItem.TopBid = bid.ToString();
        auctionListingItem.Lastbider = lastBidder;

       PlayerManager.instance.SavePlayerData();
       Task updateBidTask = FireBaseManager.instance.UpdateAuctionListingBid(auctionListingItem, PlayerManager.instance.CurrentPlayer);
       await updateBidTask;
       if(updateBidTask.IsFaulted)
       {
            Debug.Log("Error updating bid");
            //add return To Current Biddier and save
            return;
        }
        else
        {
            Debug.Log("Bid was Successful");
            //add logic to return money to previous bidder here or DB trigger
            myBid.text = "";
            ShowInfoPanel("Bid was Successful");
        }


    }

    public void UpdateSlectedListing(AuctionListing auctionListing)
    {
        if (auctionListing == null)
        {
            Debug.Log("AuctionListing is null");
            return;
        }
        if(selectedListing != null)
        {
            selectedListing.GetComponent<Image>().color = UnselectedColor;
        }
        selectedListing = auctionListing;
        selectedListing.GetComponent<Image>().color = SelectedColor;
    }

    private void ShowInfoPanel(string text)
    {
        informationPanel.SetInfoText(text);
        PlayerManager.instance.ShowInformationPanel();
    }

    private void ShowTooltip(InventorySlot slot)
    {
        Item item = slot.item;

        if (item != null && (_isTooltipActive == false || slot.isTooltipActive == false))
        {
            _isTooltipActive = true;
            slot.isTooltipActive = true;
            ResetTooltip(slot);
            PlayerManager.instance.tooltip.ShowTooltip(item, slot.transform as RectTransform);
        }
        else
        {
            _isTooltipActive = false;
            ResetTooltip(null);
            PlayerManager.instance.tooltip.HideTooltip();
        }
    }

    private void ResetTooltip(InventorySlot slot)
    {
        foreach(AuctionListing listing in auctionListings)
        {
            if (listing.inventorySlot == slot)
                continue;
            listing.inventorySlot.isTooltipActive = false;
        }
    }

    public void GetGearByType(string gearType)
    {
        // Remove all existing listings
        foreach (AuctionListing listing in auctionListings)
        {
            Destroy(listing.gameObject);
        }
        auctionListings.Clear();

        if (auctionListenerRegistration != null)
        {
            auctionListenerRegistration.Stop();
        }
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query query;
        if (gearType == "All")
        {
            query = db.Collection("auctions")
                .OrderByDescending("ExpirationTime")
                .Limit(30);
        }
        else
        {
            query = db.Collection("auctions")
                .WhereEqualTo(new FieldPath("Item", "EquipmentType"), gearType) // Add filter for GearType
                .OrderByDescending("ExpirationTime")
                .Limit(30);
        }

        auctionListenerRegistration = query.Listen(snapshot =>
            {
                foreach (DocumentChange change in snapshot.GetChanges())
                {
                    Debug.Log(change.ToString());
                    Debug.Log("Change Type: " + change.ChangeType);
                    if (change.ChangeType == DocumentChange.Type.Added)
                    {
                        AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                        AuctionListing existingListing = auctionListings.Find(listing => listing.ListingId == auctionListingItem.ListingId);
                        if (existingListing == null)
                        {
                            NewListing(auctionListingItem);
                        }
                        else
                        {
                            UpdateListing(existingListing, auctionListingItem);
                        }
                    }
                    else if (change.ChangeType == DocumentChange.Type.Modified)
                    {
                        AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                        AuctionListing existingListing = auctionListings.Find(listing => listing.ListingId == auctionListingItem.ListingId);
                        if (existingListing == null)
                        {
                            NewListing(auctionListingItem);
                        }
                        else
                        {
                            UpdateListing(existingListing, auctionListingItem);
                        }
                    }
                    else if (change.ChangeType == DocumentChange.Type.Removed)
                    {
                        AuctionListingItem auctionListingItem = change.Document.ConvertTo<AuctionListingItem>();
                        RemoveListing(auctionListingItem);
                    }
                    else
                    {
                        Debug.Log("Unknown Document Change Type");
                    }
                }
            });
    }

}
