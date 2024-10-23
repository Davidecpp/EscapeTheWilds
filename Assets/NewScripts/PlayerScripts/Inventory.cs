using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{   
    // Key
    public bool hasKey = false;
    [SerializeField] private RawImage keyImage;
    
    // Alpha image
    public float maxAlphaValue = 1f;
    public float minAlphaValue = 0.3f;

    public int maxBullets = 10;

    [System.Serializable] // Makes resource counters serializable
    public struct ResourceCounter
    {
        public int count;
        public TextMeshProUGUI counterText;
    }
    
    // Resources counters
    [SerializeField] private ResourceCounter strawberryCounter;
    [SerializeField] private ResourceCounter coinCounter;
    [SerializeField] private ResourceCounter bulletCounter;
    
    // Obj images
    [SerializeField] private Image strawberryImg;

    private void Start()
    {
        SetResource(bulletCounter);
    }

    private void Update()
    {
        SetImageAlpha(hasKey ? maxAlphaValue : minAlphaValue);
    }
    
    // Change key image aplha
    private void SetImageAlpha(float alphaValue)
    {
        if (keyImage != null)
        {
            Color currentColor = keyImage.color;
            currentColor.a = alphaValue;
            keyImage.color = currentColor;
        }
        else
        {
            Debug.LogError("RawImage reference is not set.");
        }
    }

    private void SetResource(ResourceCounter resourceCounter)
    {
        if (resourceCounter.counterText != null && resourceCounter.Equals(bulletCounter))
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count + "/" + maxBullets;
        }
    }

    public void AddResource(ref ResourceCounter resourceCounter, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (resourceCounter.Equals(bulletCounter) && resourceCounter.count >= maxBullets)
            {
                Debug.Log("MAX AMMO");
            }
            else
            {
                resourceCounter.count++;
                UpdateResourceCounter(resourceCounter);
            }
        }
    }

    private void UpdateResourceCounter(ResourceCounter resourceCounter)
    {
        if (resourceCounter.counterText != null)
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count;
            if (resourceCounter.Equals(bulletCounter))
            {
                resourceCounter.counterText.text += "/"+maxBullets;
            }
        }
        else
        {
            Debug.LogError("Resource Counter Text reference is not set.");
        }
    }

    public int GetResourceCount(ResourceCounter resourceCounter)
    {
        return resourceCounter.count;
    }

    public int GetBullets()
    {
        return bulletCounter.count;
    }

    public void RemoveBullet()
    {
        bulletCounter.count--;
        bulletCounter.counterText.text = "x " + bulletCounter.count + "/" + maxBullets;
        
    }

    public void RemoveCoins(int amount)
    {
        coinCounter.count -= amount;
        coinCounter.counterText.text = "x " + coinCounter.count;
    }

    // Wrapper methods for adding specific resources
    public void AddCoin(int amount) => AddResource(ref coinCounter, amount);
    public void AddStrawberry(int amount) => AddResource(ref strawberryCounter, amount);
    public void AddBullet(int amount) => AddResource(ref bulletCounter, amount);

    // Wrapper methods for getting specific resource counts
    public int GetCoinCount() => GetResourceCount(coinCounter);
    public int GetStrawberryCount() => GetResourceCount(strawberryCounter);
    public int GetBulletCount() => GetResourceCount(bulletCounter);
}
