using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinTxt;

    public ShopItem[] shopItems;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelGO;

    public Button[] purchaseBtns;

    private Inventory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanelGO[i].SetActive(true);
        }
        
        coinTxt.text = "Coins: " + _inventory.GetCoinCount();
        LoadPanels();
        CheckPurchaseable();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    // verifica se puoi acquistare gli oggetti
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

    public void PurchaseItem(int btNo)
    {
        if (_inventory.GetCoinCount() >= shopItems[btNo].cost)
        {
            _inventory.RemoveCoins(shopItems[btNo].cost);
            coinTxt.text = "Coins: " + _inventory.GetCoinCount();
            CheckPurchaseable();

            if (shopItems[btNo].title == "Ammo")
            {
                AddBullets(3);
            }
        }
    }

    public void AddCoins()
    {
        _inventory.AddCoin(1);
        coinTxt.text = "Coins: " + _inventory.GetCoinCount();
        CheckPurchaseable();
    }
    public void AddBullets(int amount)
    {
        _inventory.AddBullet(amount);
        CheckPurchaseable();
    }
    
    // carica info oggetti nei pannelli
    public void LoadPanels()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItems[i].title;
            shopPanels[i].itemImage.sprite = shopItems[i].itemImage;
            shopPanels[i].descriptionTxt.text = shopItems[i].description;
            shopPanels[i].costTxt.text = "Coins: " + shopItems[i].cost.ToString();
        }
    }
}
