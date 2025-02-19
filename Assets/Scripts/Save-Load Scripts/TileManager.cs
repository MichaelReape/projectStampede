using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<TileData> tiles;
    
    public List<TileData> GetTileDatas()
    {
        return tiles;
    }
}
