using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    //reptresent each cell in the grid where a tile/room can be placed
    //keep track of the cells position in the grid
    public Vector2Int gridPosition;
    //a list of possible tiles that can be placed in this cell
    public List<TileData> possibleTiles;
    //the collapsed cell, the chosen TileData
    public TileData chosenTile = null;
    //image paths at the spawers
    public string[] imagePaths = new string[4];

    public GridCell(Vector2Int gridPos, List<TileData> tiles)
    {
        gridPosition = gridPos;
        possibleTiles = new List<TileData>(tiles);
    }


}
