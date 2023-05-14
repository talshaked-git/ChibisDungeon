using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public class AuctionListingItem
{
    [FirestoreDocumentId]
    public string ListingId { get; set; }

    [FirestoreProperty]
    public string SellerId { get; set; }

    [FirestoreProperty]
    public string SellerName { get; set; }

    [FirestoreProperty]
    public ItemSaveData Item { get; set; }

    [FirestoreProperty]
    public int ItemAmount { get; set; }

    [FirestoreProperty]
    public string ItemName { get; set; }

    [FirestoreProperty]
    public string ExpirationTime { get; set; }

    [FirestoreProperty]
    public string BidderId { get; set; }

    [FirestoreProperty]
    public string TopBid { get; set; }

    [FirestoreProperty]
    public string BuyoutPrice { get; set; }

    [FirestoreProperty]
    public Dictionary<string,string> Lastbider { get; set; }


    public AuctionListingItem() { }

    public AuctionListingItem( string sellerId, string sellerName, Item item,int itemAmount, string expirationTime, string bidderId, string topBid, string buyoutPrice, Dictionary<string, string> lastbider = null)
    {
        SellerId = sellerId;
        SellerName = sellerName;
        Item = new ItemSaveData(item);
        ItemAmount = itemAmount;
        ItemName = item.ItemName;
        ExpirationTime = expirationTime;
        BidderId = bidderId;
        TopBid = topBid;
        BuyoutPrice = buyoutPrice;
        Lastbider = lastbider;
    }

    public int GetBuyoutPrice()
    {
        return int.Parse(BuyoutPrice);
    }
}
