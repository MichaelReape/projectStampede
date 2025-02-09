using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//this data strucure will hold the data of the unique map
//will be serialized and stored in a json file

[Serializable]
public class RoomData
{
    //holds the postion of the room in the map
    public Vector3 position;
    //holds the type of the room
    public string roomType;
}

[Serializable]
public class MapData
{
    //Stores all the room data
    public List<RoomData> rooms = new List<RoomData>();
}

