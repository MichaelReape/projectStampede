using System.Collections;
using System.Collections.Generic;
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
    private int gridx;
    private int gridy;
    private int buttonIndex;
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
        //get the text from the input field
        string prompt = promptInput.text;
        //int gridx = ButtonController.getGridX();

        //get the image from the API
        if (!prompt.Equals(""))
        {
            ButtonController.SetButtonRed();
            APIManager.APIInstance.GetImageFromAPI(prompt,gridx, gridy, buttonIndex,(Sprite result) =>
            {
                Debug.Log(gridx + " " + gridy + " " + buttonIndex);
                Debug.Log("Image received from API");
                image.sprite = result;
                ButtonController.SetButtonGreen();
            });
        }
        else
        {
            ButtonController.SetButtonGreen();
            Debug.Log("Prompt is empty");
        }
        //close the prompt canvas
        promptCanvas.SetActive(false);
        Debug.Log("Prompt Canvas Closed");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
    }
    public void OnCancel()
    {
        Debug.Log("Cancel Button Clicked");
        ButtonController.SetButtonGreen();
        //close the prompt canvas
        promptCanvas.SetActive(false);
        Debug.Log("Prompt Canvas Closed");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
    }
}
