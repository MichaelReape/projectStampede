using System.Collections;
using System.Collections.Generic;
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
    //[SerializeField] public Image image;
    //[SerializeField] private GameObject promptCanvas;
    public PromptCanvasController promptCanvasController;
    //[SerializeField] private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform;
    }

    private void OnMouseDown()
    { 
        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= interactionDistance)
        {
            Debug.Log("Click");
            //APIManager.APIInstance.GetImageFromAPI("A big,fat, orange cat smoking a bong", (Sprite result) =>
            //{
            //    image.sprite = result;
            //});


            //going to call the prompt canvas and set it to active
            promptCanvasController.gameObject.SetActive(true);
            //promptCanvas.SetActive(true);
            promptCanvasController.setButtonInfo(gridx, gridy, buttonIndex);
            //promptCanvasController.buttonIndex = buttonIndex;
            //promptCanvasController.gridx = gridx;
            //promptCanvasController.gridy = gridy;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Prompt Canvas Active");
            PlayerMovement.PlayerMovementInstance.CanMove = false;

            //engage teh button animation
            Vector3 newPosition = buttonTransform.position;
            newPosition.y -= 0.05f;
            buttonTransform.position = newPosition;
        }

    }
    private void OnMouseUp()
    {
        //disengage the button animation
        Vector3 newPosition = buttonTransform.position;
        newPosition.y += 0.05f;
        buttonTransform.position = newPosition;
        Debug.Log("Clack");
    }

    public void setGrid(int x, int y)
    {
        gridx = x;
        gridy = y;
    }
    public int getGridX()
    {
        return gridx;
    }
    public int getGridY()
    {
        return gridy;
    }
    public void SetButtonRed()
    {
        Debug.Log("Red");
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void SetButtonGreen()
    {
        Debug.Log("Green");
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
