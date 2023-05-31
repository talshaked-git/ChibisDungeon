using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public List<Loot> lootList = new List<Loot>();
    private float dropForce = 1.5f;

    private List<Loot> GetDropedItems()
    {
        int randomNumber;
        int amountOfItemToDrop;
        List<Loot> dropedItems = new List<Loot>();

        foreach (Loot loot in lootList)
        {
            randomNumber = Random.Range(1, 101);
            if (randomNumber <= loot.chance)
            {
                amountOfItemToDrop = Random.Range(1, loot.amount + 1);
                loot.amount = amountOfItemToDrop;
                dropedItems.Add(loot);
            }
        }

        return dropedItems;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> loots = GetDropedItems();

        if(loots == null || loots.Count == 0)
        {
            return;
        }

        foreach (Loot loot in loots)
        {
            for(int i = 0; i < loot.amount; i++)
            {
                GameObject lootGameObject = Instantiate(GameAssets.i.pfLoot, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<Pickup>().SetPickupItem(loot.item);
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f)).normalized;
                lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Force);
            }
        }
    }


}
