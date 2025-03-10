using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//class to save the map data to a file in the persistent data path
public class MapSaver : MonoBehaviour
{
    //string to hold the directory path
    public string directory;
    //function to save the map data to a file
    public void SaveGrid(MapData map, string fileName)
    {
        //convert the map data to a json string
        string json = JsonUtility.ToJson(map);
        //create the directory path
        directory = Path.Combine(Application.persistentDataPath, "Saves");
        //check if the directory exists
        if (!Directory.Exists(directory))
        {
            //if it doesn't exist create it
            Directory.CreateDirectory(directory);
        }
        //create the file path
        string path = Path.Combine(directory, fileName + ".json");

        //write the json to the file
        File.WriteAllText(path, json);

        //confirm in console that the file has been saved
        Debug.Log("File " + fileName + " saved to " + path);
    }
}
