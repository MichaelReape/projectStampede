using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class PromptCanvasController : MonoBehaviour
{

    public TMP_InputField promptInput;
    public GameObject promptCanvas;
    public Image image;
    public ButtonController ButtonController;
    private int gridx;
    private int gridy;
    private int buttonIndex;
    public int type;

    //for setting the button location on the grid and in the room, used for saving the image and loading it back in the correct location
    public void setButtonInfo(int x, int y, int index)
    {
        gridx = x;
        gridy = y;
        buttonIndex = index;
    }
    public void OnConfirm()
    {
        //grabs the input text
        string prompt = promptInput.text;
        //type 1 is 2d image
        if (type == 1)
        {
            //reset the pause menu flag on confirm
            PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);

            //get the image from the API
            //prompt cant be empty and no api call in progress
            if (!prompt.Equals("") && !APIManager.APIInstance.isCallingAPI)
            {
                //button animation colour
                ButtonController.SetButtonRed();
                //call the api manager to get the image
                APIManager.APIInstance.GetImageFromAPI(prompt, gridx, gridy, buttonIndex, (Sprite result) =>
                {
                    //set the image to the returned sprite
                    image.sprite = result;
                    //button animation colour green
                    ButtonController.SetButtonGreen();
                    //button animation, pops up again
                    buttonAnimation();
                });
            }
            else
            {
                //button animation, pops up again
                buttonAnimation();
                //print error
                Debug.Log("Prompt is empty");
            }

            //close the prompt canvas
            promptCanvas.SetActive(false);
            //hide cursor again and player can move
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
        //type 2 is 3d object
        else if (type == 2)
        {
            //reset the pause menu flag on confirm
            PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);

            //code for 3d prompt to api controller
            //cant be empty and no api call in progress
            if (!prompt.Equals("") && !APIManager.APIInstance.isCallingAPI)
            {
                //button animation colour
                ButtonController.SetButtonRed();
                //call the api manager to get the 3d object
                //will return a path to the object
                //object loader will instantiate the glb file with the gltf plugin
                APIManager.APIInstance.Get3DObjectFromAPI(prompt, (string localPath) =>
                {
                    //prompt is the file name
                    string filePath = Path.Combine(localPath, prompt);
                    filePath = filePath + ".glb";
                    //load the 3d object
                    ObjectLoader.ObjectLoaderInstance.LoadObject(filePath, prompt);
                    //button animation colour green
                    ButtonController.SetButtonGreen();
                    //button animation, pops up again
                    buttonAnimation();
                });
            }
            else
            {
                buttonAnimation();
                Debug.Log("Prompt is empty");
            }
            //close the prompt canvas
            promptCanvas.SetActive(false);
            //hide cursor again and player can move
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
    }
    public void OnCancel()
    {
        //reset the pause menu flag on cancel
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
        //button animation colour
        ButtonController.SetButtonGreen();
        //close the prompt canvas
        promptCanvas.SetActive(false);
        //hide cursor again and player can move
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
        //button animation, pops up again
        buttonAnimation();
    }
    //helper for the button animations
    public void buttonAnimation()
    {
        //if the button flag is set
        if (ButtonController.isButtonPressed)
        {
            //raise the button back up and reset the flag
            Vector3 newPosition = ButtonController.buttonTransform.position;
            newPosition.y += 0.05f;
            ButtonController.buttonTransform.position = newPosition;
            ButtonController.isButtonPressed = false;
        }
    }
}
