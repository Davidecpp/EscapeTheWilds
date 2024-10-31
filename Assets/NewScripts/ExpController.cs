using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _levelTxt;
    [SerializeField]private TextMeshProUGUI _experienceTxt;
    [SerializeField] private Image ExpProgressBar;

    private PlayerStats _playerStats;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        _experienceTxt.text = _playerStats.GetExperience() + "/" + _playerStats.GetNextLevelExp();
        ExperienceController();
    }

    public void ExperienceController()
    {
        _levelTxt.text = _playerStats.GetLevel().ToString();
        ExpProgressBar.fillAmount = _playerStats.GetExperience() / _playerStats.GetNextLevelExp();
    }
}
