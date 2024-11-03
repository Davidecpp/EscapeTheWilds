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

    private string[] lines;
    private int index;
    public bool isActive;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        textComponent.text = string.Empty;
        isActive = false;
        //SetDialogue(new string[] { "Ho bisogno di fare qualche soldo!", "Sfruttiamo un po' di animali.." });
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
        }
        else
        {
            SetImageAlpha(box,0f);  
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
        
        //_gameManager.PauseGame();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
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
        _gameManager.ResumeGame();
    }
}