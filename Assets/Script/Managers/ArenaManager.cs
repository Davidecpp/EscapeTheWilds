using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Round
    public int round = 1;               // current round
    public TextMeshProUGUI roundTxt;    // Round text

    // Update is called once per frame
    void Update()
    {
        // Update round txt
        roundTxt.gameObject.SetActive(GameManager.Instance.arenaMode); // set round txt active if arenaMode is  true
        roundTxt.text = "Round " + round; // Update text
    }
}