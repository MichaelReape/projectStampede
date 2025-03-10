using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clss to hold all the tile prefabs
public class TileManager : MonoBehaviour
{
    //list of all the tile prefabs
    public List<TileData> tiles;
    //return the list of tile prefabs
    public List<TileData> GetTileDatas()
    {
        return tiles;
    }
}
