using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// using System.Runtime.CompilerServices;
// using UnityEngine.SceneManagement;

//class to load the map data from a file in the persistent data path
public class MapLoader : MonoBehaviour
{
    //reference to the load menu canvas
    public GameObject LoadMenuCanvas;
    //lock the player movement while choosing a map to load
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.PlayerMovementInstance.CanMove = false;
    }

    public void LoadMap(string fileName)
    {
        //disable the load menu
        LoadMenuCanvas.SetActive(false);

        //call the load grid function
        MapData map = LoadGrid(fileName);
        //set the map name
        MapManager.MapManagerInstance.mapName = fileName;
        //reconstruct the grid from the map data loaded
        MapManager.MapManagerInstance.ReconstructGrid(map);
        //cycle through the objects from the map and instantiate them
        foreach (ObjectData objectData in map.objects)
        {
            //instantiate the object
            ObjectLoader.ObjectLoaderInstance.LoadObjectFromSave(objectData);
        }
    }

    //helper method to load the map data from a file
    public MapData LoadGrid(string fileName)
    {
        //get the path to the file from the filename passed from the load menu
        string path = Path.Combine(Application.persistentDataPath, "Saves", fileName + ".json");

        //check if the file exists or not
        if (!File.Exists(path))
        {
            Debug.LogError("File " + fileName + " does not exist");
            return null;
        }

        //read the json from the file
        string json = File.ReadAllText(path);

        //deserialize the json into a MapData object
        MapData map = JsonUtility.FromJson<MapData>(json);
        //return the map object
        return map;
    }

    //method to get the list of maps in the save directory
    //used to populate the load menu
    public string[] GetMaps()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        //if the directory does not exist create it and return null to the load menu list
        if (!Directory.Exists(saveDirectory))
        {
            Debug.LogError("Directory " + saveDirectory + " does not exist");
            Directory.CreateDirectory(saveDirectory);
            return null;
        }
        //get all the files in the directory with the .json extension
        string[] maps = Directory.GetFiles(saveDirectory, "*.json");
        return maps;
    }
}