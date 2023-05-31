using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChest : MonoBehaviour
{
    Animator animator;
    public List<Loot> lootList = new List<Loot>();
    public bool isLooted = false;
    public bool isInRange = false;
    private PlayerManager playerManager;
    private Button openChestButton; // Add a reference to the button
    private float dropForce = 3.5f;
    private Vector3 offSetVector = new Vector3(0,2.5f);


    private void Start()
    {
        animator = GetComponent<Animator>();
        openChestButton = GameObject.Find("Attack").GetComponent<Button>(); // Get the button component
        openChestButton.onClick.AddListener(OnOpenChestButtonPressed);
    }

    private void OnOpenChestButtonPressed()
    {
        if (isInRange && !isLooted)
        {
            animator.SetTrigger("Open");
            StartCoroutine(OpenChest(transform.position));
            isLooted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other.gameObject, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CheckCollision(other.gameObject, false);
    }

    private void CheckCollision(GameObject gameObject, bool state)
    {
        if (gameObject.CompareTag("Player"))
        {
            isInRange = state;
        }
    }

    private IEnumerator OpenChest(Vector3 spawnPosition)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedChest"));
        InstantiateLoot(spawnPosition + offSetVector);
    }

    private void InstantiateLoot(Vector3 spawnPosition)
    { 

        if (lootList == null || lootList.Count == 0)
        {
            return;
        }

        foreach (Loot loot in lootList)
        {
            for (int i = 0; i < loot.amount; i++)
            {
                GameObject lootGameObject = Instantiate(GameAssets.i.pfLoot, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<Pickup>().SetPickupItem(loot.item);
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f)).normalized;
                lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Force);
            }
        }
    }

    // Clean up the event listener when the object is destroyed
    private void OnDestroy()
    {
        openChestButton.onClick.RemoveListener(OnOpenChestButtonPressed);
    }
}
