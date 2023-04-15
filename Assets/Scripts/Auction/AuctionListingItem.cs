using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionListingItem
{
    public Item item;
    public string itemName;
    public string timeLeft;
    public string sellerName;
    public string currentBid;
    public string buyoutPrice;

    public AuctionListingItem(Item item, string timeLeft, string sellerName, string currentBid, string buyoutPrice)
    {
        this.item = item;
        this.itemName = item.ItemName;
        this.timeLeft = timeLeft;
        this.sellerName = sellerName;
        this.currentBid = currentBid;
        this.buyoutPrice = buyoutPrice;
    }


}
