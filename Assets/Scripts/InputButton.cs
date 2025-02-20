using System;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
public class InputButton : MonoBehaviour
{
    public GameObject inputPanel;
    public InputFieldController inputFieldController;
    public float interactionDistance = 2.0f;
    //private Transform player;


    //fix this later, want player to not move while canvas/button is open
    //private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        inputPanel = GameObject.Find("InputPanel");
        ////inputFieldController = inputPanel.GetComponent<InputFieldController>();
        if (inputPanel != null)
        {
            inputPanel.SetActive(false);
            Debug.Log("Found inputPanel");
            //inputPanel.SetActive(true);
        }
        //test();

        //inputPanel.SetActive(false);

        //player = Camera.main.transform;
        //menu is invisible until updated
        //menuCanvas.SetActive(false

        //inputFieldController.open();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        // playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        GameObject panel = GameObject.Find("InputPanel");
        Debug.Log("Click");
        if (panel != null)
        {
            panel.SetActive(true);
            inputFieldController = panel.GetComponent<InputFieldController>();
            if (inputFieldController != null)
            {
                inputFieldController.open();
            }
        }
        //inputPanel.SetActive(true);

        //if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
        //{
        //if the player is close enough to activate it
        //menuCanvas.SetActive(true);
        //inputPanel.SetActive(true);
        //inputFieldController.open();        
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        //}
    }
    // void OnMouseUp()
    // {
    //     menuCanvas.SetActive(false);
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }

    void test()
    {
        Debug.Log("Test");
        
    }
}
