using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject saveMenuPanel;
    //public TMP_InputField saveMapNameInput;

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

    // Update is called once per frame
    void Update()
    {
        //check for m key to bring up menu
        if(Input.GetKeyDown(KeyCode.M) && !isPauseMenuOpen)
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
            //or lock movement
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
    }

    public void OnResumeButtonClicked()
    {
        Debug.Log("Resume button clicked");
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
        Debug.Log("Main Menu button clicked");
        //SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
    public void OnConfirmButtonClicked()
    {
        //string mapName = saveMapNameInput.text;
        //Debug.Log("Save button clicked with map name: " + mapName);
        
        Debug.Log("Map name is: " + MapManager.MapManagerInstance.mapName);
        //save map
        MapManager.MapManagerInstance.saveMap();
        saveMenuPanel.SetActive(false);
        pauseMenu.SetActive(true);
        //reset map name
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
