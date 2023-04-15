using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionListing : MonoBehaviour
{
    [SerializeField]
    TMP_Text itemName;
    [SerializeField]
    TMP_Text timeLeft;
    [SerializeField]
    TMP_Text sellerName;
    [SerializeField]
    TMP_Text currentBid;
    [SerializeField]
    TMP_Text buyoutPrice;


    public void SetListing(AuctionListingItem listing)
    {
        itemName.text = listing.itemName;
        timeLeft.text = listing.timeLeft;
        sellerName.text = listing.sellerName;
        currentBid.text = listing.currentBid;
        buyoutPrice.text = listing.buyoutPrice;
    }

    
}
