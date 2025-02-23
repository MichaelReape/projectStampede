using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    private Transform player;
    //[SerializeField] public Image image;
    [SerializeField] private GameObject promptCanvas;
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
            promptCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Prompt Canvas Active");
            PlayerMovement.PlayerMovementInstance.CanMove = false;

        }

    }
    private void OnMouseUp()
    {
        Debug.Log("Clack");
    }
}
