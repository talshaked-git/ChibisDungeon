using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Item item;

    public void SetPickupItem(Item item)
    {
        this.item = item.GetCopy();
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (PlayerManager.instance.AddItem(item))
            {
                Debug.Log("Item Looted");
            }
            else
            {
                Debug.Log("Inventory is full");
                Debug.Log("Item Sent To Mail Inbox");
            }
            Destroy(gameObject);
        }
    }
}
