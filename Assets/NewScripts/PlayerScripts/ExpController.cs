using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _levelTxt;
    [SerializeField]private TextMeshProUGUI _experienceTxt;
    [SerializeField] private Image ExpProgressBar;

    private float loadedExp;

    private PlayerStats _playerStats;

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
        Debug.Log("LoadedExp"+ loadedExp);
    }
    private void SetPlayerReference(PlayerStats player)
    {
        _playerStats = player;
    }

    private void ExperienceController()
    {
        _levelTxt.text = _playerStats.GetLevel().ToString();
        ExpProgressBar.fillAmount = _playerStats.GetExperience() / _playerStats.GetNextLevelExp();
        loadedExp = _playerStats.GetExperience();
    }

    public void LoadStats(float exp)
    {
        exp = loadedExp;
    }
}
