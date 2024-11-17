using System;
using TMPro;
using UnityEngine;

public class RaceManger : MonoBehaviour
{
    public TextMeshProUGUI title;       // Title text

    void Update()
    {   
        title.gameObject.SetActive(GameManager.Instance.raceMode); // set title txt active if raceMode is  true

    }

}