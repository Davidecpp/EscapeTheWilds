using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Round
    public int round = 1;
    public TextMeshProUGUI roundTxt;

    // Update is called once per frame
    void Update()
    {
        // Update round txt
        roundTxt.gameObject.SetActive(GameManager.Instance.arenaMode);
        roundTxt.text = "Round " + round;
    }
}