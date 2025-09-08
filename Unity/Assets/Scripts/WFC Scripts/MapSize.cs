using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//class for setting the map size
public class MapSize : MonoBehaviour
{
    public TMP_InputField mapSizeXInput;
    public TMP_InputField mapSizeYInput;

    //static variables to be accessed across scenes
    public static int mapSizeX;
    public static int mapSizeY;

    //set the map size, called from the button
    public void SetXY()
    {
        if (mapSizeXInput.text != null && mapSizeYInput.text != null)
        {
            mapSizeX = int.Parse(mapSizeXInput.text);
            mapSizeY = int.Parse(mapSizeYInput.text);

            //load the scene
            SceneManager.LoadScene("NewMapTemplate");
        }
        else
        {
            Debug.Log("Map size input is null");
        }
    }
}
