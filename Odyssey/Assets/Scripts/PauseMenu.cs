using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public MouseLook look;

    public GameObject pauseMenu;
    
    public Slider sensitivitySlider;
    public Text sensitivityText;

    public GameObject pauseButtons;
    public GameObject settingsMenu;

    // Update is called once per frame
    void Update()
    {
        if (CrystalInteract.gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Settings()
    {
        Debug.Log("Settings");
        pauseButtons.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    
    //SETTINGS
    
    public void Back()
    {
        settingsMenu.SetActive(false);
        pauseButtons.SetActive(true);
    }

    public void SetSensitivity()
    {
        look.mouseSensitivity = sensitivitySlider.value;
        sensitivityText.text = "Sensitivity: " + look.mouseSensitivity;
    }
}
