using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : ScriptableObject
{
    //holds the data of the tile
    public string tileName;
    //position of doors to be stored, can be 1 per wall, so 0 for no door, 1 for 
    //door positions will be stored in the order of south, west, north, east
    public List<int> doorPositions = new List<int> { 0, 0, 0, 0 };

    //holds the prefab of the tile
    public GameObject tilePrefab;
}
