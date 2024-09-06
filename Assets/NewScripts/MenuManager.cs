using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject buttons;
    public GameObject gameModes;
    public GameObject controls;
    public GameObject menu;

    private void Start()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogError("ESC");

            // Alterna lo stato del menu
            if (menu.activeSelf)
            {
                if (gameModes.activeSelf)
                {
                    OpenMainButtons();
                }
                else
                {
                    menu.SetActive(false);
                    controls.SetActive(false);
                    Time.timeScale = 1;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                
            }
            else
            {
                menu.SetActive(true);  
                OpenMainButtons();  
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            
        }
    }

    public void PlayArena(int i)
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    public void PlayFootball(int i)
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayRace()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void OpenGameModes()
    {
        buttons.SetActive(false);
        gameModes.SetActive(true);
    }

    public void OpenMainButtons()
    {
        buttons.SetActive(true);
        gameModes.SetActive(false);
    }

    public void OpenControls()
    {
        controls.SetActive(true);
        menu.SetActive(false);
    }
}