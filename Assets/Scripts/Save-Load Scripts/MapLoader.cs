using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    //public TileManager tileManager;
    //public List<TileData> tiles;
    public GameObject LoadMenuCanvas;

    private void Start()
    {
        //    //canr remember if this is needed
        //    tiles = tileManager.GetTileDatas();
        //    //Debug.Log("Number of tiles loaded " + tiles.Count);
        //    //MapData map = LoadGrid("testMap");
        //    //MapManager.MapManagerInstance.ReconstructGrid(map);
        //    //Debug.Log("Map loaded");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerMovement.PlayerMovementInstance.CanMove = false;
    }

public void LoadMap(string fileName)
    {
        //disable the load menu
        LoadMenuCanvas.SetActive(false);
        //load the scene first
        //SceneManager.LoadScene("LoadMapTemplate");
        //open the cursonr and lock the movement
        MapData map = LoadGrid(fileName);
        MapManager.MapManagerInstance.mapName = fileName;
        MapManager.MapManagerInstance.ReconstructGrid(map);
        Debug.Log("Map loaded");
    }

    public MapData LoadGrid(string fileName)
    {
        //get the path to the file
        Debug.Log("Loading file " + fileName);
        string path = Path.Combine(Application.persistentDataPath, "Saves", fileName + ".json");
        Debug.Log("Path to file " + path);

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
        Debug.Log("File " + fileName + " loaded from " + path);
        return map;
    }

    public string[] GetFiles()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(saveDirectory))
        {
            Debug.LogError("Directory " + saveDirectory + " does not exist");
            Directory.CreateDirectory(saveDirectory);
            return null;
        }
        string[] files = Directory.GetFiles(saveDirectory, "*.json");
        return files;
        //may get rid of the extension later
    }

    //public void InstantiateLoadedgrid(MapData map)
    //{
    //    //Debug.Log(map.rooms[0].imagePaths[0]);
    //    //possibly use a dictionary to store the tiles and their names
    //    Debug.Log("Instantiating loaded grid");

    //    //first handle if the grid is null
    //    if (map == null)
    //    {
    //        Debug.LogError("MapData is null");
    //        return;
    //    }

    //    int cellSize = map.cellSize;
    //    //iterate through the list of rooms
    //    foreach (RoomData room in map.rooms)
    //    {
    //        foreach (TileData tile in tiles)
    //        {
    //            if (tile.tileName.Equals(room.roomType))
    //            {
    //                //instantiate the tile
    //                GameObject tilePrefab = Instantiate(tile.tilePrefab, new Vector3(room.x * cellSize, 0, room.y * cellSize), Quaternion.identity);
    //                break;
    //            }
    //        }
    //    }

    //}
}