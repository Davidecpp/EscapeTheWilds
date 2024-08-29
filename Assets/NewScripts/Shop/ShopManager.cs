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
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanelGO[i].SetActive(true);
        }
        coinTxt.text = "Coins: " + coins.ToString();
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
            if (coins >= shopItems[i].cost)
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
        if (coins >= shopItems[btNo].cost)
        {
            coins -= shopItems[btNo].cost;
            coinTxt.text = "Coins: " + coins.ToString();
            CheckPurchaseable();
        }
    }

    public void AddCoins()
    {
        coins++;
        coinTxt.text = "Coins: " + coins.ToString();
        CheckPurchaseable();
    }
    
    // carica info oggetti nei pannelli
    public void LoadPanels()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItems[i].title;
            shopPanels[i].descriptionTxt.text = shopItems[i].description;
            shopPanels[i].costTxt.text = "Coins: " + shopItems[i].cost.ToString();
        }
    }
}
