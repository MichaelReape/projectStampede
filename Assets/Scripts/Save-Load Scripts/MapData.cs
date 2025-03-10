using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData
{
    //store an x and y position in the grid
    public int x;
    public int y;
    //store the roomtype
    public string roomType;
    //Array of the images saves in the rorom
    //order bottom left, top left, top right, bottom right
    public string[] imagePaths = new string[4];
}
[Serializable]
public class ObjectData
{
    //store the name of the 3Dobject
    public string name;
    //store the position, rotation and scale of the object
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public class MapData
{
    //store the dimensions of the grid
    public int width;
    public int height;
    public int cellSize;

    //data structure to store the room data objects
    public List<RoomData> rooms = new List<RoomData>();
    //data structure to store the 3d object data
    public List<ObjectData> objects = new List<ObjectData>();
}



