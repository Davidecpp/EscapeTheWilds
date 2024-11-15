using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public Image box;

    public GameObject skip;

    private string[] lines;
    private int index;
    public bool isActive;

    void Start()
    {
        textComponent.text = string.Empty;
        isActive = false;
    }

    void Update()
    {
        // left-mouse-button or enter for next dialogue line
        if (isActive && (Input.GetMouseButtonDown(0) || Keyboard.current.enterKey.wasPressedThisFrame))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        HideBox();
    }
    
    // Hide the box when not in use
    private void HideBox()
    {
        if (isActive)
        {
            SetImageAlpha(box, 1f);
            skip.SetActive(true);
        }
        else
        {
            SetImageAlpha(box,0f);  
            skip.SetActive(false);
        }
    }
    // Change image alpha
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    
    // Set new dialogue
    public void SetDialogue(string[] newLines)
    {
        if (isActive) return;

        lines = newLines;
        index = 0;
        textComponent.text = string.Empty;
        isActive = true;
        
        GameManager.Instance.PauseGame();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed); // for ignoring timescale so txt animation doesn't stop
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isActive = false;
        textComponent.text = string.Empty;
        GameManager.Instance.ResumeGame();
    }
    public void CheckInitialDialogue()
    {
        if (GameManager.Instance.currentScene == 4)
        {
            SetDialogue(new[] { "Enter the house.", "Press WASD to move." });
        }
    }
}