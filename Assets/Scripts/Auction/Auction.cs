using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auction : MonoBehaviour
{
    [SerializeField]
    List<AuctionListing> auctionListings = new List<AuctionListing>();
    [SerializeField]
    GameObject auctionListingPanel;
    [SerializeField]
    GameObject auctionListingPrefab;

    // public void Start()
    // {
    //     List<AuctionListingItem> auctionListingItems =  FireBaseManager.instance.GetAuctionListingItems();
    //     foreach (AuctionListingItem listingItem in auctionListingItems)
    //     {
    //         GameObject listingObject = Instantiate(auctionListingPrefab, auctionListingPanel.transform, auctionListingPanel);
    //         listingObject.GetComponent<AuctionListing>().SetListing(listingItem);
    //         auctionListings.Add(listingObject.GetComponent<AuctionListing>());
    //     }
    // }

    public void AddListing(AuctionListingItem listingItem)
    {
        GameObject listingObject = Instantiate(auctionListingPrefab, auctionListingPanel.transform, auctionListingPanel);
        listingObject.GetComponent<AuctionListing>().SetListing(listingItem);
        auctionListings.Add(listingObject.GetComponent<AuctionListing>());
    }

    public void RemoveListing(AuctionListing listing)
    {
        auctionListings.Remove(listing);
        Destroy(listing.gameObject);
    }
}
