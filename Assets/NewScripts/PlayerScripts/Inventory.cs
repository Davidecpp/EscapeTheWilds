using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public bool hasKey = false;
    [SerializeField] private RawImage keyImage;
    public float maxAlphaValue = 1f;
    public float minAlphaValue = 0.3f;

    private int _strawberryCount;
    [SerializeField] private TextMeshProUGUI strawberryCounterText;

    private void Update()
    {
        if (hasKey)
        {
            SetImageAlpha(maxAlphaValue);
        }
        else
        {
            SetImageAlpha(minAlphaValue);
        }
    }

    private void SetImageAlpha(float alphaValue)
    {
        if (keyImage != null)
        {
            Color currentColor = keyImage.color;
            currentColor.a = alphaValue;
            keyImage.color = currentColor;
        }
        else
        {
            Debug.LogError("RawImage reference is not set.");
        }
    }

    public void AddStrawberry()
    {
        _strawberryCount++;
        UpdateStrawberryCounter();
    }

    private void UpdateStrawberryCounter()
    {
        if (strawberryCounterText != null)
        {
            strawberryCounterText.text = "x " + _strawberryCount;
        }
        else
        {
            Debug.LogError("Strawberry Counter Text reference is not set.");
        }
    }

    public int GetStrawberryCount()
    {
        return _strawberryCount;
    }
}