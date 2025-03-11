using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    public int buttonIndex;
    private int gridx;
    private int gridy;
    private Transform player;
    public Transform buttonTransform;
    public PromptCanvasController promptCanvasController;
    public bool isButtonPressed = false;
    public TMP_Dropdown dropdown;
    public GameObject dropdownCanvas;

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform;
    }

    private void OnMouseDown()
    {
        //player must be within the set distance, pause menu must be closed, and there cannot be any api call inprogress
        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= interactionDistance && !PauseMenuController.PMCInstance.GetIsPauseMenuOpen() && !APIManager.APIInstance.isCallingAPI)
        {
            //pause menu flag to true, gets reset on cancel or submit
            PauseMenuController.PMCInstance.SetIsPauseMenuOpen(true);

            //lock player movement and show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMovement.PlayerMovementInstance.CanMove = false;
            //show the selection dropdown
            dropdownCanvas.SetActive(true);
            //for selecting the option, 2d or 3d
            dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
            //button animation
            if (!isButtonPressed)
            {
                //lower teh button and set the flag
                Vector3 newPosition = buttonTransform.position;
                newPosition.y -= 0.05f;
                buttonTransform.position = newPosition;
                isButtonPressed = true;
            }
        }

    }
    //calls corresponding method to the dropdown value
    private void DropdownValueChanged(TMP_Dropdown change)
    {
        if (change.value == 1)
        {
            Option2D();
        }
        else if (change.value == 2)
        {
            Option3D();
        }
    }
    //2d image option 
    public void Option2D()
    {
        //the type is 1 for 2d image
        promptCanvasController.type = 1;
        //opens the prompt canvas
        promptCanvasController.gameObject.SetActive(true);
        //gives the button info to know from which spawner the prompt is made
        promptCanvasController.setButtonInfo(gridx, gridy, buttonIndex);
        //closes the dropdown
        dropdownCanvas.SetActive(false);
    }
    public void Option3D()
    {
        //the type is 2 for 3d object
        promptCanvasController.type = 2;
        //opens the prompt canvas
        promptCanvasController.gameObject.SetActive(true);
        //closes the dropdown
        dropdownCanvas.SetActive(false);
    }
    //logs the room coordinates for the button, from the mapmanager
    public void setGrid(int x, int y)
    {
        gridx = x;
        gridy = y;
    }
    //dont think i used these in the end, but good to have
    public int getGridX()
    {
        return gridx;
    }
    public int getGridY()
    {
        return gridy;
    }
    //helper for the button animation, will be red while call in progress
    public void SetButtonRed()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    //helper for the button animation, will be green when call is done
    public void SetButtonGreen()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
