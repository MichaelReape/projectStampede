using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//this will hold the data of the unique map
//will be serialized and stored in a json file

[Serializable]
public class RoomData
{
    //if i add rotation to the rooms, i will add it here
    //store an x and y position
    public int x;
    public int y;
    //store the room type
    public string roomType;
}

[Serializable]
public class MapData
{
    //might need to store the grid size
    public int width;
    public int height;
    //Stores all the room data
    //need to flatten 2d array of rooms into a list
    //simple enough just iterate through the 2d array and add each room to the list
    public List<RoomData> rooms = new List<RoomData>();
}

