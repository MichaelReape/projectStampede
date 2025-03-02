using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class PromptCanvasController : MonoBehaviour
{

    [SerializeField] private TMP_InputField promptInput;
    [SerializeField] private GameObject promptCanvas;
    [SerializeField] public Image image;
    //public Transform buttonTransform;
    public ButtonController ButtonController;
    //public PauseMenuController pauseMenuController;
    private int gridx;
    private int gridy;
    private int buttonIndex;

    public int type;
    //need to reference the player movement script
    //[SerializeField] private PlayerMovement playerMovement;

    //on submit button click

    public void setButtonInfo(int x, int y, int index)
    {
        gridx = x;
        gridy = y;
        buttonIndex = index;
    }
    public void OnConfirm()
    {
        Debug.Log("Confirm Button Clicked");
            string prompt = promptInput.text;
        if(type == 1)
        {
            Debug.Log("2D Option, type = " + type);

            //get the text from the input field
            //int gridx = ButtonController.getGridX();
            PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
            //pauseMenuController.SetIsPauseMenuOpen(false);

            //get the image from the API
            if (!prompt.Equals("") && !APIManager.APIInstance.isCallingAPI)
            {
                ButtonController.SetButtonRed();
                APIManager.APIInstance.GetImageFromAPI(prompt,gridx, gridy, buttonIndex,(Sprite result) =>
                {
                    Debug.Log(gridx + " " + gridy + " " + buttonIndex);
                    Debug.Log("Image received from API");
                    image.sprite = result;
                    ButtonController.SetButtonGreen();
                    buttonAnimation();
                });
                //if (APIManager.APIInstance.isCallingAPI)
                //{
                //    ButtonController.SetButtonGreen();
                //    buttonAnimation();
                //    Debug.Log("API call in progress");
                //} 
            }
            else
            {
                //ButtonController.SetButtonGreen();
                buttonAnimation();
                Debug.Log("Prompt is empty");
            }
            //close the prompt canvas
            promptCanvas.SetActive(false);
            Debug.Log("Prompt Canvas Closed");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
        else if (type == 2)
        {
            Debug.Log("3D Option, type = " + type);
            Debug.Log("promp is " + prompt);
            PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
            //string prompt = promptInput.text;
            //code for 3d prompt to api controller
            if (!prompt.Equals("") && !APIManager.APIInstance.isCallingAPI)
            {
                ButtonController.SetButtonRed();
                APIManager.APIInstance.Get3DObjectFromAPI(prompt, (string localPath) =>

                {
                    Debug.Log("3D Object received from API");
                    //load the 3d object
                    string filePath = Path.Combine(localPath, prompt);
                    filePath = filePath + ".glb";
                    ObjectLoader.ObjectLoaderInstance.LoadObject(filePath, prompt);
                    //i should probably save it too here
                    ButtonController.SetButtonGreen();
                    buttonAnimation();
                });


                //Debug.Log("3D Object received from API");
                ////load the 3d object
                //ObjectLoader.ObjectLoaderInstance.LoadObject(objectPath);
                ////i should probably save it too here
                //ButtonController.SetButtonGreen();
                //buttonAnimation();
            }
            else
            {
                buttonAnimation();
                Debug.Log("Prompt is empty");
            }
            //close the prompt canvas
            promptCanvas.SetActive(false);
            Debug.Log("Prompt Canvas Closed");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.PlayerMovementInstance.CanMove = true;
        }
    }
    public void OnCancel()
    {
        //pauseMenuController.SetIsPauseMenuOpen(false);
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
        Debug.Log("Cancel Button Clicked");
        ButtonController.SetButtonGreen();
        //close the prompt canvas
        promptCanvas.SetActive(false);
        Debug.Log("Prompt Canvas Closed");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
        buttonAnimation();
    }
    public void buttonAnimation()
    {
        if (ButtonController.isButtonPressed)
        {
            Vector3 newPosition = ButtonController.buttonTransform.position;
            newPosition.y += 0.05f;
            ButtonController.buttonTransform.position = newPosition;
            ButtonController.isButtonPressed = false;
        }
    }
}
