using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMap : MonoBehaviour
{
    // Start is called before the first frame 
    void Start()
    {
        MapManager.MapManagerInstance.InitialiseGrid(2, 2);
        Debug.Log("grid initialised");
        MapManager.MapManagerInstance.RunWFC();
        Debug.Log("WFC run");

        //need a way to take in the grid dimensions from teh main menu scene

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
