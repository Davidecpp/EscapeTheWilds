using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{   
    // Key: The player's status regarding key possession
    public bool hasKey = false;
    
    // Max bullet capacity
    public int maxBullets = 10;
    
    // Reference to the CanvasManager for UI updates
    private CanvasManager _canvasManager;

    [System.Serializable] // Makes resource counters serializable so they can be displayed in the inspector
    public struct ResourceCounter
    {
        public int count;  // The current count of the resource
        public TextMeshProUGUI counterText; // The UI text element to display the resource count
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Find and store the CanvasManager instance
        _canvasManager = FindObjectOfType<CanvasManager>();

        // Initialize bullet count display
        SetBullets(_canvasManager.bulletCounter);

        // If the current scene is 4 (e.g., a specific game level), set coin count to 0
        if (GameManager.Instance.currentScene == 4)
        {
            SetCoinCount(0);
        }
    }

    // Set the bullet counter UI display with the current count and max bullets
    private void SetBullets(ResourceCounter resourceCounter)
    {
        // Check if the counterText is set and update the bullet count display
        if (resourceCounter.counterText != null && resourceCounter.Equals(_canvasManager.bulletCounter))
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count + "/" + maxBullets;
        }
    }

    // Add a certain amount of resources to a given resource counter (e.g., bullets, coins)
    public void AddResource(ref ResourceCounter resourceCounter, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Check if it's the bullet counter and if the max bullet capacity is reached
            if (resourceCounter.Equals(_canvasManager.bulletCounter) && resourceCounter.count >= maxBullets)
            {
                Debug.Log("MAX AMMO");  // Log message if the max bullets is reached
            }
            else
            {
                // Increment the resource count and update the UI
                resourceCounter.count++;
                UpdateResourceCounter(resourceCounter);
            }
        }
    }

    // Update the resource counter's UI text
    private void UpdateResourceCounter(ResourceCounter resourceCounter)
    {
        // Check if the counterText is not null and update the display
        if (resourceCounter.counterText != null)
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count;
            // If it's the bullet counter, also display the max bullets
            if (resourceCounter.Equals(_canvasManager.bulletCounter))
            {
                resourceCounter.counterText.text += "/" + maxBullets;
            }
        }
        else
        {
            Debug.LogError("Resource Counter Text reference is not set.");  // Log an error if the counterText is not set
        }
    }

    // Get the current resource count for a specific resource counter
    public int GetResourceCount(ResourceCounter resourceCounter)
    {
        return resourceCounter.count;
    }

    // Get the current bullet count
    public int GetBullets()
    {
        return _canvasManager.bulletCounter.count;
    }

    // Decrease the bullet count by one and update the UI
    public void RemoveBullet()
    {
        _canvasManager.bulletCounter.count--;
        _canvasManager.bulletCounter.counterText.text = "x " + _canvasManager.bulletCounter.count + "/" + maxBullets;
    }

    // Decrease the coin count by a specified amount and update the UI
    public void RemoveCoins(int amount)
    {
        _canvasManager.coinCounter.count -= amount;
        _canvasManager.coinCounter.counterText.text = "x " + _canvasManager.coinCounter.count;
    }

    // Set the coin count to a specific amount and update the UI
    public void SetCoinCount(int amount)
    {
        _canvasManager.coinCounter.count = amount;
        UpdateResourceCounter(_canvasManager.coinCounter);
    }
    // Set the strawberry count to a specific amount and update the UI
    public void SetStrawberryCount(int amount)
    {
        _canvasManager.strawberryCounter.count = amount;
        UpdateResourceCounter(_canvasManager.strawberryCounter);
    }

    // Wrapper methods for adding specific resources
    public void AddCoin(int amount) => AddResource(ref _canvasManager.coinCounter, amount);
    public void AddStrawberry(int amount) => AddResource(ref _canvasManager.strawberryCounter, amount);
    public void AddBullet(int amount) => AddResource(ref _canvasManager.bulletCounter, amount);

    // Wrapper methods for getting specific resource counts
    public int GetCoinCount() => GetResourceCount(_canvasManager.coinCounter);
    public int GetStrawberryCount() => GetResourceCount(_canvasManager.strawberryCounter);
    public int GetBulletCount() => GetResourceCount(_canvasManager.bulletCounter);
}
