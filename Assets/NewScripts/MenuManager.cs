using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject buttons;
    public GameObject gameModes;
    public GameObject controls;
    public GameObject options;
    public GameObject menu;

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

    private void ResumeGame()
    {
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

    public void CloseGameModes()
    {
        gameModes.SetActive(false);
        buttons.SetActive(true);
    }

    public void OpenControls()
    {
        controls.SetActive(true);
        menu.SetActive(false);
    }

    public void OpenOptions()
    {
        buttons.SetActive(false);
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        options.SetActive(false);
        buttons.SetActive(true);
    }
}