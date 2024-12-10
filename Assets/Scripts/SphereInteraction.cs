using System;
using System.Xml;
using UnityEngine;
public class SphereInteraction : MonoBehaviour
{
    public GameObject menuCanvas;
    public float interactionDistance = 2.0f;
    private Transform player;


    //fix this later, want player to not move while canvas/button is open
    //private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform;
        //menu is invisible until updated
        menuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
        {
            //if the player is close enough to activate it
            menuCanvas.SetActive(true);
            Debug.Log("Click");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    // void OnMouseUp()
    // {
    //     menuCanvas.SetActive(false);
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }
}
