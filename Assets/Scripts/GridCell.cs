using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    //will reptresent each cell in the grid where a tile/room can be placed
    //will keep track of the cells position in the grid
    public Vector2Int gridPosition;
    //a list of possible tiles that can be placed in this cell
    public List<TileData> possibleTiles ;
    //the collapsed cell, the chosen TileData
    public TileData chosenTile = null;
    
    public GridCell(Vector2Int gridPos, List<TileData> tiles)
    {
        gridPosition = gridPos;
        possibleTiles = new List<TileData>(tiles);
    }


}
