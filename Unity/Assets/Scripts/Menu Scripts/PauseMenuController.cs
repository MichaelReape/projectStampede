using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject saveMenuPanel;

    private bool isPauseMenuOpen = false;
    private static PauseMenuController instance;

    public static PauseMenuController PMCInstance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        //singleton pattern to ensure only one instance of the WFCManager
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        saveMenuPanel.SetActive(false);
    }

    void Update()
    {
        //check for m key to bring up menu
        if (Input.GetKeyDown(KeyCode.M) && !isPauseMenuOpen)
        {
            Debug.Log("M key pressed");
            isPauseMenuOpen = true;
            pauseMenu.SetActive(true);
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        //lock movement
        if (isPauseMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMovement.PlayerMovementInstance.CanMove = false;
        }
        else
        {
            //unlock movement
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
    }

    public void OnResumeButtonClicked()
    {
        isPauseMenuOpen = false;
        pauseMenu.SetActive(false);
        TogglePauseMenu();
    }
    public void OnSaveButtonClicked()
    {
        saveMenuPanel.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClicked()
    {
        //not working in editor
        //would work in a build
        Application.Quit();
    }
    public void OnConfirmButtonClicked()
    {
        //save map
        MapManager.MapManagerInstance.saveMap();
        saveMenuPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void OnCancelButtonClicked()
    {
        saveMenuPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void SetIsPauseMenuOpen(bool value)
    {
        isPauseMenuOpen = value;
    }
    public bool GetIsPauseMenuOpen()
    {
        return isPauseMenuOpen;
    }
}
