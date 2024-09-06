using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject esc;
    public GameObject buttons;
    public GameObject gameModes;
    public GameObject controls;
    public GameObject options;
    public GameObject characterSelection;
    public GameObject menu;
    
    public GameObject[] characters;

    private void Start()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Debug.Log("RESUMEE");
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.LogError("ESC");

            // Alterna lo stato del menu
            if (menu.activeSelf)
            {
                if (gameModes.activeSelf)
                {
                    CloseGameModes();
                }
                else if (options.activeSelf)
                {
                    CloseOptions();
                }
                else if (controls.activeSelf)
                {
                    CloseControls();
                }
                else if(characterSelection.activeSelf)
                {
                    CloseCharacterSelection();
                }
                else
                {
                    menu.SetActive(false);
                    controls.SetActive(false);
                    ResumeGame();
                }
            }
            else
            {
                controls.SetActive(false);
                menu.SetActive(true);  
                CloseGameModes();  
                PauseGame();
            }
        }

        if (buttons.activeSelf)
        {
            esc.SetActive(false);
        }
        else
        {
            esc.SetActive(true);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Play(int i)
    {
        // 1 ARENA
        // 2 FOOTBALL
        // 3 RACE
        SceneManager.LoadSceneAsync(i);
    }

    public void OpenGameModes()
    {
        buttons.SetActive(false);
        gameModes.SetActive(true);
    }

    private void CloseGameModes()
    {
        gameModes.SetActive(false);
        buttons.SetActive(true);
    }

    public void OpenControls()
    {
        controls.SetActive(true);
        buttons.SetActive(false);
        //menu.SetActive(false);
    }

    private void CloseControls()
    {
        controls.SetActive(false);
        buttons.SetActive(true);
    }

    public void OpenOptions()
    {
        buttons.SetActive(false);
        options.SetActive(true);
    }

    private void CloseOptions()
    {
        options.SetActive(false);
        buttons.SetActive(true);
    }

    public void OpenCharacterSelection()
    {
        gameModes.SetActive(false);
        characterSelection.SetActive(true);
    }
    private void CloseCharacterSelection()
    {
        gameModes.SetActive(true);
        characterSelection.SetActive(false);
    }
    public void SelectCharacter(int characterID)
    {
        // Salva l'ID del personaggio selezionato
        PlayerPrefs.SetInt("SelectedCharacter", characterID);
        Debug.Log("Selected: " + characterID);

        // Cambia la scena dopo aver selezionato il personaggio
        SceneManager.LoadScene("Arena");
    }

}