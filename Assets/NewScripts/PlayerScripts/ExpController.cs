using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _levelTxt;
    [SerializeField]private TextMeshProUGUI _experienceTxt;
    [SerializeField] private Image ExpProgressBar;

    private PlayerStats _playerStats;

    private void Start()
    {
        PlayerPrefs.DeleteKey("PlayerExp");
        PlayerPrefs.DeleteKey("PlayerLevel");
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = FindObjectOfType<PlayerStats>();
            if (_playerStats != null)
            {
                SetPlayerReference(_playerStats);
            }
            else
            {
                return;
            }
        }
        _experienceTxt.text = _playerStats.GetExperience() + "/" + _playerStats.GetNextLevelExp();
        ExperienceController();
    }
    private void SetPlayerReference(PlayerStats player)
    {
        _playerStats = player;
    }

    private void ExperienceController()
    {
        _levelTxt.text = _playerStats.GetLevel().ToString();
        ExpProgressBar.fillAmount = _playerStats.GetExperience() / _playerStats.GetNextLevelExp();
    }

}
