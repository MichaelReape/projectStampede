using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "WFC/TileData")]
public class TileData : ScriptableObject
{
    //holds the data of the tile
    public string tileName;
    //number of doors will be used to determine the type of the room
    public int numberOfDoors; //can maybe be calculated from doorPositions
    //position of doors to be stored, can be 1 per wall, so 0 for no door, 1 for 
    //door positions will be stored in the order of south, west, north, east
    //might change later
    public List<int> doorPositions = new List<int> { 0, 0, 0, 0 };

    //adds a weight to the tile, this will be used to determine the probability of the tile being selected
    public float wieght = 1f;

    //holds the prefab of the tile
    public GameObject tilePrefab;
}
