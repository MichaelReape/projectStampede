using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;

public class MapLoader : MonoBehaviour
{
    public TileManager tileManager;
    public List<TileData> tiles;

    private void Start()
    {
        tiles = tileManager.GetTileDatas();
        Debug.Log("Number of tiles loaded " + tiles.Count);
        MapData map = LoadGrid("testMap");
        InstantiateLoadedgrid(map);
        Debug.Log("Map loaded");
    }

    public MapData LoadGrid(string fileName)
    {
        //get the path to the file
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

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

    public void InstantiateLoadedgrid(MapData map)
    {
        //possibly use a dictionary to store the tiles and their names


        //first handle if the grid is null
        if (map == null)
        {
            Debug.LogError("MapData is null");
            return;
        }

        int cellSize = map.cellSize;
        //iterate through the list of rooms
        foreach (RoomData room in map.rooms)
        {
            foreach (TileData tile in tiles)
            {
                if (tile.tileName.Equals(room.roomType))
                {
                    //instantiate the tile
                    GameObject tilePrefab = Instantiate(tile.tilePrefab, new Vector3(room.x * cellSize, 0, room.y * cellSize), Quaternion.identity);
                    break;
                }
            }
        }

    }
}