using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("Item UI")]
    public GameObject itemUI; // The parent object containing all item images
    public Image[] itemSlots; // Array of 3 item images (assign in inspector)

    [Header("Item Sprites")]
    public Sprite[] emptyItemSprites; // Sprites for uncollected items (3 different empty sprites)
    public Sprite[] collectedItemSprites; // Sprites for collected items (3 different items)

    private bool[] itemsCollected = new bool[3]; // Track which items are collected
    private bool itemUIUnlocked = false; // Whether the item UI has been shown

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize all items as not collected
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemsCollected[i] = false;
            if (itemSlots[i] != null && emptyItemSprites.Length > i)
            {
                itemSlots[i].sprite = emptyItemSprites[i];
            }
        }
    }

    // Call this after a specific dialogue to show the item UI
    public void UnlockItemUI()
    {
        itemUIUnlocked = true;
        if (itemUI != null)
        {
            itemUI.SetActive(true);
        }
    }

    // Call this when an item is collected (0, 1, or 2)
    public void CollectItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= 3)
        {
            Debug.LogWarning("Invalid item index: " + itemIndex);
            return;
        }

        if (!itemsCollected[itemIndex])
        {
            itemsCollected[itemIndex] = true;

            // Update the UI sprite
            if (itemSlots[itemIndex] != null && collectedItemSprites.Length > itemIndex)
            {
                itemSlots[itemIndex].sprite = collectedItemSprites[itemIndex];
            }

            // Play item collect sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayItemCollect();
            }

            Debug.Log("Item " + itemIndex + " collected!");
        }
    }

    // Check if all items are collected
    public bool AllItemsCollected()
    {
        for (int i = 0; i < itemsCollected.Length; i++)
        {
            if (!itemsCollected[i])
                return false;
        }
        return true;
    }

    // Check if a specific item is collected
    public bool IsItemCollected(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= 3)
            return false;
        return itemsCollected[itemIndex];
    }

    // Get number of items collected
    public int GetCollectedItemCount()
    {
        int count = 0;
        for (int i = 0; i < itemsCollected.Length; i++)
        {
            if (itemsCollected[i])
                count++;
        }
        return count;
    }

    // Hide the item UI (e.g., during ending sequence)
    public void HideItemUI()
    {
        if (itemUI != null)
        {
            itemUI.SetActive(false);
        }
    }

    // Show the item UI
    public void ShowItemUI()
    {
        if (itemUI != null && itemUIUnlocked)
        {
            itemUI.SetActive(true);
        }
    }
}