using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewMap : MonoBehaviour
{
    public GameObject newMapPanel;
    public TMP_InputField mapNameInput;
    //public GameObject pauseMenuPanel;
    public PauseMenuController pauseMenuPanel;
    // Start is called before the first frame 
    void Start()
    {
        newMapPanel.SetActive(true);
        MapManager.MapManagerInstance.InitialiseGrid(2, 2);
        Debug.Log("grid initialised");
        MapManager.MapManagerInstance.RunWFC();
        Debug.Log("WFC run");

        //unlcok the mouse and lock the movement
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.PlayerMovementInstance.CanMove = false;
        //need a way to take in the grid dimensions from teh main menu scene
        pauseMenuPanel.SetIsPauseMenuOpen(true);
    }

    public void OnConfirmButtonClicked()
    {
        if (mapNameInput != null)
        {
            MapManager.MapManagerInstance.mapName = mapNameInput.text;
            MapManager.MapManagerInstance.saveMap();
            Debug.Log("map saved as " + mapNameInput);
            newMapPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
            pauseMenuPanel.SetIsPauseMenuOpen(false);
        }
        else
        {
            Debug.Log("map name input is null");
        }
    }
}
