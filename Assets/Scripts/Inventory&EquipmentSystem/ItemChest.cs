using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemChest : MonoBehaviour
{
    Animator animator;
    public Item item;
    public bool isLooted = false;
    public bool isInRange = false;
    private PlayerManager playerManager;
    private ArcherAttack attackButton;
    private Button openChestButton; // Add a reference to the button


    private void Start()
    {
        animator = GetComponent<Animator>();
        playerManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<PlayerManager>();
        openChestButton = GameObject.Find("Attack").GetComponent<Button>(); // Get the button component
        openChestButton.onClick.AddListener(OnOpenChestButtonPressed);
    }

    private void OnOpenChestButtonPressed()
    {
        if (isInRange && !isLooted)
        {
            if (!playerManager.AddItem(Instantiate(item)))
                return;
            isLooted = true;
            item = null;
            animator.SetTrigger("Open");
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

    // Clean up the event listener when the object is destroyed
    private void OnDestroy()
    {
        openChestButton.onClick.RemoveListener(OnOpenChestButtonPressed);
    }
}
