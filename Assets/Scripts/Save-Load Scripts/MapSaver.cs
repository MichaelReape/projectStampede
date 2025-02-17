using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MapSaver : MonoBehaviour
{
    public void SaveGrid(MapData map, string fileName)
    {
        string json = JsonUtility.ToJson(map);

        //may need to check if already exists and also to create the directory
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        //print path to console
        Debug.Log("Saving to: " + path);

        //write the json to the file
        File.WriteAllText(path, json);

        //print to console that the file has been saved
        Debug.Log("File " + fileName + " saved to " + path);
    }
}
