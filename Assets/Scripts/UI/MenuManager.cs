using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{ 

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject settingsMenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            bool state = pauseMenu.gameObject.activeSelf;

            if (state == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    private void SetAllInactive()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }   
    
    void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void OpenMenu(Button defaultMenuButton)
    {
        SetAllInactive();
        defaultMenuButton.transform.parent.gameObject.SetActive(true);
        defaultMenuButton.Select();
    }
    
    
    public void ResumeGame()
    {
        SetAllInactive();
        
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
    
     
    
}