using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// quando vai su create esce una nuova voce
[CreateAssetMenu(fileName = "shopMenu", menuName = "Scriptable Objects/New shop Item", order = 1)]
public class ShopItem : ScriptableObject
{
    public string title;
    public string description;
    public int cost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
