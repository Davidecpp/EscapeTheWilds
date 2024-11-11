using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TMP_Text coinTxt;
    
    // Shop
    public ShopItem[] shopItems;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelGO;
    public Button[] purchaseBtns;

    private Inventory _inventory;
    private PlayerStats _playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        _playerStats = FindObjectOfType<PlayerStats>();
        
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanelGO[i].SetActive(true);
        }
        
        LoadPanels();
        CheckPurchaseable();
    }

    // Update is called once per frame
    void Update()
    {
        coinTxt.text = "Coins: " + _inventory.GetCoinCount();
        CheckPurchaseable();
    }
    
    // Check if you can buy an item
    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (_inventory.GetCoinCount() >= shopItems[i].cost)
            {
                purchaseBtns[i].interactable = true;
            }
            else
            {
                purchaseBtns[i].interactable = false;
            }
        }
    }
    // Buy items
    public void PurchaseItem(int btNo)
    {
        if (_inventory.GetCoinCount() >= shopItems[btNo].cost)
        {
            _inventory.RemoveCoins(shopItems[btNo].cost);

            if (shopItems[btNo].title == "Ammo")
            {
                AddBullets(3);
            }
            if (shopItems[btNo].title == "Heart")
            {
                _playerStats.AddHeart();
            }
        }
    }
    // Add +1 coin
    public void AddCoins()
    {
        _inventory.AddCoin(1);
    }
    // Add amount bullets
    public void AddBullets(int amount)
    {
        _inventory.AddBullet(amount);
    }
   
    
    // Load objects info in the shop panels
    public void LoadPanels()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItems[i].title;
            shopPanels[i].itemImage.sprite = shopItems[i].itemImage;
            shopPanels[i].descriptionTxt.text = shopItems[i].description;
            shopPanels[i].costTxt.text = "Coins: " + shopItems[i].cost;
        }
    }
}
