using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MapSize : MonoBehaviour
{
    public TMP_InputField mapSizeXInput;
    public TMP_InputField mapSizeYInput;

    public static int mapSizeX;
    public static int mapSizeY;

    public void SetXY()
    {
        if (mapSizeXInput.text != null && mapSizeYInput.text != null)
        {
            mapSizeX = int.Parse(mapSizeXInput.text);
            mapSizeY = int.Parse(mapSizeYInput.text);
            Debug.Log("Map size set to " + mapSizeX + " x " + mapSizeY);

            //load the scene
            SceneManager.LoadScene("NewMapTemplate");
        }
        else
        {
            Debug.Log("Map size input is null");
        }
    }
}
