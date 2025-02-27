using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MapSaver : MonoBehaviour
{
    public string directory;
    public void SaveGrid(MapData map, string fileName)
    {
        string json = JsonUtility.ToJson(map);

        directory = Path.Combine(Application.persistentDataPath, "Saves");
        //check if the directory exists
        if (!Directory.Exists(directory))
        {
            //if it doesn't exist create it
            Directory.CreateDirectory(directory);
        }
        //may need to check if already exists and also to create the directory
        string path = Path.Combine(directory, fileName + ".json");

        //print path to console
        Debug.Log("Saving to: " + path);

        //write the json to the file
        File.WriteAllText(path, json);

        //print to console that the file has been saved
        Debug.Log("File " + fileName + " saved to " + path);
    }
}
