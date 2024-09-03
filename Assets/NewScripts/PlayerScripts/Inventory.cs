using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public bool hasKey = false;
    [SerializeField] private RawImage keyImage;
    public float maxAlphaValue = 1f;
    public float minAlphaValue = 0.3f;

    public int maxBullets = 3;

    [System.Serializable]
    public struct ResourceCounter
    {
        public int count;
        public TextMeshProUGUI counterText;
    }

    [SerializeField] private ResourceCounter strawberryCounter;
    [SerializeField] private ResourceCounter coinCounter;
    [SerializeField] private ResourceCounter bulletCounter;

    private void Start()
    {
        SetResource(bulletCounter);
    }

    private void Update()
    {
        SetImageAlpha(hasKey ? maxAlphaValue : minAlphaValue);
    }

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
        if (resourceCounter.Equals(bulletCounter))
        {
            resourceCounter.counterText.text = "x " + resourceCounter.count + "/" + maxBullets;
        }
    }

    public void AddResource(ref ResourceCounter resourceCounter)
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

    // Wrapper methods for adding specific resources
    public void AddCoin() => AddResource(ref coinCounter);
    public void AddStrawberry() => AddResource(ref strawberryCounter);
    public void AddBullet() => AddResource(ref bulletCounter);

    // Wrapper methods for getting specific resource counts
    public int GetCoinCount() => GetResourceCount(coinCounter);
    public int GetStrawberryCount() => GetResourceCount(strawberryCounter);
    public int GetBulletCount() => GetResourceCount(bulletCounter);
}
