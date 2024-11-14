using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{   
    // Key
    public bool hasKey = false;
    public int maxBullets = 10;
    private CanvasManager _canvasManager;

    [System.Serializable] // Makes resource counters serializable
    public struct ResourceCounter
    {
        public int count;
        public TextMeshProUGUI counterText;
    }
    private void Start()
    {
        _canvasManager = FindObjectOfType<CanvasManager>();
        SetBullets(_canvasManager.bulletCounter);

        if (GameManager.Instance.currentScene == 4)
        {
            SetCoinCount(0);
        }
    }

    private void SetBullets(ResourceCounter resourceCounter)
    {
        if (resourceCounter.counterText != null && resourceCounter.Equals(_canvasManager.bulletCounter))
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count + "/" + maxBullets;
        }
    }

    public void AddResource(ref ResourceCounter resourceCounter, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (resourceCounter.Equals(_canvasManager.bulletCounter) && resourceCounter.count >= maxBullets)
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
            if (resourceCounter.Equals(_canvasManager.bulletCounter))
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
        return _canvasManager.bulletCounter.count;
    }

    public void RemoveBullet()
    {
        _canvasManager.bulletCounter.count--;
        _canvasManager.bulletCounter.counterText.text = "x " + _canvasManager.bulletCounter.count + "/" + maxBullets;
        
    }

    public void RemoveCoins(int amount)
    {
        _canvasManager.coinCounter.count -= amount;
        _canvasManager.coinCounter.counterText.text = "x " + _canvasManager.coinCounter.count;
    }

    public void SetCoinCount(int amount)
    {
        Debug.Log("Coin set to "+ amount);
        _canvasManager.coinCounter.count = amount;
        UpdateResourceCounter(_canvasManager.coinCounter);
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
