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
    //need to reference the player movement script
    //[SerializeField] private PlayerMovement playerMovement;

    //on submit button click
    public void OnConfirm()
    {
        Debug.Log("Confirm Button Clicked");
        //get the text from the input field
        string prompt = promptInput.text;
        //get the image from the API
        if(!prompt.Equals(""))
        {
            APIManager.APIInstance.GetImageFromAPI(prompt, (Sprite result) =>
            {
                Debug.Log("Image received from API");
                image.sprite = result;
            });
        }
        else
        {
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
        //close the prompt canvas
        promptCanvas.SetActive(false);
        Debug.Log("Prompt Canvas Closed");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
    }
}
