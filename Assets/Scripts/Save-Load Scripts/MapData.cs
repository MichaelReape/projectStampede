using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    //store the room ,maybe change to roomName
    public string roomType;
    //Array of the images saves in the rorom
    //order bottom left, top left, top right, bottom right
    public string[] imagePaths = new string[4];
}
[System.Serializable]
public class ObjectData
{
    //store the name of the object
    public string name;
    //store the position, rotation and scale of the object
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public class MapData
{
    //might need to store the grid size
    public int width;
    public int height;
    public int cellSize;
    //Stores all the room data
    //need to flatten 2d array of rooms into a list
    //simple enough just iterate through the 2d array and add each room to the list
    public List<RoomData> rooms = new List<RoomData>();
    //data structure to store the 3d object data
    public List<ObjectData> objects = new List<ObjectData>();
}



