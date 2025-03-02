using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
   public GameObject menuCanvas;
//    public PlayerMovement playerMovement;

   public void CloseMenu(){
    menuCanvas.SetActive(false);
    Debug.Log("Close Button");
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    //code for player movement here to re-enable it 
   }
}
