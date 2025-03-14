using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewMap : MonoBehaviour
{
    public GameObject newMapPanel;
    public TMP_InputField mapNameInput;
    public PauseMenuController pauseMenuPanel;
    void Start()
    {
        //panel to name the map
        newMapPanel.SetActive(true);
        //initialise the grid with the dimensions from main menu scene
        int mapSizeX = MapSize.mapSizeX;
        int mapSizeY = MapSize.mapSizeY;

        MapManager.MapManagerInstance.RunWFC(mapSizeX, mapSizeY);

        //unlock the mouse and lock the movement
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.PlayerMovementInstance.CanMove = false;
        //pause menu flag so m doesnt open it
        pauseMenuPanel.SetIsPauseMenuOpen(true);
    }

    public void OnConfirmButtonClicked()
    {
        if (mapNameInput != null)
        {
            //sets the map name
            MapManager.MapManagerInstance.mapName = mapNameInput.text;
            //saves the map under the name
            MapManager.MapManagerInstance.saveMap();
            //closes the panel and hides the mouse
            newMapPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
            //resets the pause menu flag so m can open it
            pauseMenuPanel.SetIsPauseMenuOpen(false);
        }
        else
        {
            Debug.Log("map name input is null");
        }
    }
}
