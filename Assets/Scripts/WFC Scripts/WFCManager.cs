using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    //grid dimensions can be set
    public int gridWidth;
    public int gridHeight;
    //all rooms have the same size at the minute
    public int cellSize;
    public List<TileData> tiles;
    //2d array to hold the grid
    public GridCell[,] grid;

    //directions for the neighbours
    public Vector2Int[] directions = new Vector2Int[]
    {
        //north, east, south, west
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };

    void Start()
    {
        Debug.Log("WFC Manager started");
        InitialiseGrid();
        RunWFC();
        saveMap();
    }

    //initialise the grid
    public void InitialiseGrid()
    {
        //print the direction offsets
        foreach (Vector2Int direction in directions)
        {
            Debug.Log(direction);
        }
        //i want to print the tiles to the console
        //foreach (TileData tile in tiles)
        //{
        //    Debug.Log(tile.tileName);
        //}
        //ok so it has the tiles

        //create the grid with the given dimensions
        grid = new GridCell[gridWidth, gridHeight];
        //populate the grid with 
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new GridCell(new Vector2Int(x,y), tiles);
                //i want to print the possible tiles for each cell
                //foreach (TileData tile in grid[x, y].possibleTiles)
                //{
                //    Debug.Log(tile.tileName);
                //}
            }
        }
        Debug.Log("Grid initialised");
    }

    //collapse the grid using the WFC algorithm
    //basically loop until each cell has only one possible tile

    public void RunWFC()
    {
        bool allCollapsed = false;
        while (!allCollapsed)
        {
            //get the cell with the lowest entropy
            GridCell lowestEntropyCell = GetLowestEntropy();
            //collapse the cell
            CollapseCell(lowestEntropyCell);
        //propagate the constraints
        Propagate(lowestEntropyCell);
            //check if all cells have collapsed
            allCollapsed = CheckAllCollapsed();
            //if not repeat
        }
        //print the grid
        foreach (GridCell cell in grid)
        {
            Debug.Log("the chosen tile is " + cell.chosenTile.tileName);
        }
        Debug.Log("All cells have collapsed");
        //instantiate the tiles
        InstantiateTiles();
    }


    //returns a GridCell object with the lowest entropy
    public GridCell GetLowestEntropy()
    {
        //i think we cycle through the grid and get the cell with the lowest amount of possiblities
        //this will be the cell with the lowest entropy
        GridCell lowestEntropyCell = null;

        // loop through the grid
        foreach (GridCell cell in grid)
        {
            // if the cell has not collapsed
            if (cell.chosenTile == null)
            {
                //if the lowestEntropyCell is null, set it to the current cell
                if (lowestEntropyCell == null)
                {
                    lowestEntropyCell = cell;
                }
                //if the current cell has less possible tiles than the lowestEntropyCell
                else if (cell.possibleTiles.Count < lowestEntropyCell.possibleTiles.Count)
                {
                    lowestEntropyCell = cell;
                }
            }
        }
        //print the lowest entropy cell options
        //foreach (TileData tile in lowestEntropyCell.possibleTiles)
        //{
        //    Debug.Log("the lowest cell options are "+tile.tileName);
        //}
        //print the lowest entropy cell position
        Debug.Log("the lowest cell position is " + lowestEntropyCell.gridPosition);
        return lowestEntropyCell;
    }



    public void CollapseCell(GridCell cell)
    {
        //i want to create an empty list instead of clearing and adding to the original
        List<TileData> possibleTilesTemp = new List<TileData>();
        //if cell is already collapsed, return
        if (cell.chosenTile != null)
        {
            Debug.Log("cell already collapsed");
            return;
        }
        if (cell.possibleTiles.Count == 0)
        {
            Debug.LogWarning("No possible tiles left for cell at " + cell.gridPosition);
            //TODO: Handle contradiction, e.g. backtracking or returning early.
            //causing the game to crash
            //return;
        }
        //select a random tile from the possible tiles
        int randomTileIndex = UnityEngine.Random.Range(0, cell.possibleTiles.Count);
        //seems to tend to 0
        //print the random Tile index
        //Debug.Log("the random tile index is " + randomTileIndex);
        //print the random tile
        //Debug.Log("the random tile is " + cell.possibleTiles[randomTileIndex].tileName);
        //set the chosen tile to the random tile
        Debug.Log(randomTileIndex);
        cell.chosenTile = cell.possibleTiles[randomTileIndex];

        //print the chosen tile      
        Debug.Log("The cell is collapsed, the chosen tile is " + cell.chosenTile.tileName);
        //remove all other possible tiles
        //cell.possibleTiles.Clear();
        //set the chosen tile to the cell list of possible tiles (tidying up)
        //cell.possibleTiles.Add(cell.chosenTile);

        //add the chosen tile to the temp list
        //possibleTilesTemp.Add(cell.chosenTile);
        //set the possible tiles to the temp list
        cell.possibleTiles = new List<TileData>{cell.chosenTile};
        //possibleTilesTemp.Clear();
    }
    public void Propagate(GridCell cell)
    {
        if (cell.chosenTile == null)
        {
            Debug.LogWarning("Tried to propagate from a cell that isn't collapsed: " + cell.gridPosition);
            return;
        }
        Debug.Log("Propagating constraints " + cell.gridPosition);
        //1. get the neighbours of the cell
        foreach (Vector2Int direction in directions)
        {
            //print the direction
            Debug.Log("the direction we are looking is " + direction);
            //get the neighbour using the direction offset
            Vector2Int neighbourPos = cell.gridPosition + direction;
            //check if the neighbour is within the grid
            if (IsValidPosition(neighbourPos))
            {
                //print here
                Debug.Log("the neighbour position is " + neighbourPos + " and is valid");
                //get the neighbour cell
                GridCell neighbour = grid[neighbourPos.x, neighbourPos.y];

                //if the neighbour hasnt collapsed, ie it has a list to update
                if (neighbour.chosenTile == null)
                {
                    //the doors should lineup
                    //if the neighbour is to the north for example
                    //the north door of the cell should be the same as the south door of the neighbour
                    //so we remove based on the doors of the source cell
                    //if there is no door then obviously there wont be any neighbours in that direction
                    //since we are going through each direction we can use an if else to decide which wall to chekc
                    //print here
                    Debug.Log("the neighbour has not collapsed");
                    int sourceWallIndex = 0;
                    int neighbourWallIndex = 0;

                    //little confusing because the order for doors is south, west, north, east
                    //but for directions it is north, east, south, west
                    //might change later
                    if (direction == directions[0])
                    {
                        //north neighbour
                        //their south door should be the same as our north door
                        sourceWallIndex = 2;
                        neighbourWallIndex = 0;
                        Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[1])
                    {
                        //east neighbour
                        sourceWallIndex = 3;
                        neighbourWallIndex = 1;
                        Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[2])
                    {
                        //south neighbour
                        sourceWallIndex = 0;
                        neighbourWallIndex = 2;
                        Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[3])
                    {
                        //west neighbour
                        sourceWallIndex = 1;
                        neighbourWallIndex = 3;
                        Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    //Debug.Log("sourceWallIndex: " + sourceWallIndex);
                    //Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    //Debug.Log("Direction: " + direction);
                    //now need to filter the possible tiles based on the doors

                    List<TileData> tilesToRemove = new List<TileData>();
                    foreach (TileData tile in neighbour.possibleTiles)
                    {
                        //this may be wrong, need to check
                        //right now it only checks the source cell doors but what about the walls
                        //might be a feature as it will create hallways idk

                        //issue is the cell.chosenTile might be null
                        //not sure about this
                        //if(cell.chosenTile == null)
                        //{
                        //    return;
                        //}
                        //if the source cell has a door in the direction
                        //if (cell.chosenTile != null)
                        //{ }
                            if (cell.chosenTile.doorPositions[sourceWallIndex] == 1 && tile.doorPositions[neighbourWallIndex]==0)
                            {
                            //if the neighbour tile doesnt have a door in the opposite 
                            //remove the tile from the list of possible tiles

                            //this might be a bad idea as it could cause a runtime error
                            //instead i might create a copy list and just overwrite the original at the end
                            //neighbour.possibleTiles.Remove(tile);
                            tilesToRemove.Add(tile);
                            }
                    }
                    foreach (TileData tile in tilesToRemove)
                    {
                        neighbour.possibleTiles.Remove(tile);
                    }
                    //recursively call the propagate method on the neighbour
                    Propagate(neighbour);
                }
            }
        }
    }


    //checks if the position is within the grid
    public bool IsValidPosition(Vector2Int pos)
    {
        //print the position
        //Debug.Log("the position is " + pos);
        if (pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight)
        {
            return true;
        }
        return false;
    }


    //this will check if all cells have collapsed
    public bool CheckAllCollapsed()
    {
        //loop throught the grid and check if cell.chosenTile is null
        foreach (GridCell cell in grid)
        {
            if (cell.chosenTile == null)
            {
                return false;
            }
        }
        return true;
    }

    //last thing to do is to create a method to instantiate the tiles
    //this will be done after the grid has been collapsed
    public void InstantiateTiles()
    {
        //this might be wrong, need to check
        //is instantiating the tiles with a gap between them
        //maybe need to change the cell size to 1

        //loop through the grid and instantiate the tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //print the chosen tile
                Debug.Log("the chosen tile is " + grid[x, y].chosenTile.tileName + " at position " + x + ", " + y);
                //get the chosen tile
                TileData chosenTile = grid[x, y].chosenTile;
                //instantiate the tile
                GameObject tile = Instantiate(chosenTile.tilePrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
            }
        }
        Debug.Log("Tiles instantiated");
    }

    //code to test the save feature
    //DO NOT FORGET TO REMOVE!!!
    [SerializeField] private MapSaver mapSaver;
    private MapData generatedData;

   public void saveMap()
    {
        //create a new mapData object
        MapData mapData = new MapData();
        mapData.height = gridHeight;
        mapData.width = gridWidth;
        mapData.cellSize = cellSize;
        //flatten the 2d array of grid cells into a list
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridCell cell = grid[x, y];
                //create a new roomData object
                RoomData roomData = new RoomData();
                roomData.x = x;
                roomData.y = y;
                roomData.roomType = cell.chosenTile.tileName;
                Debug.Log("the room type is " + roomData.roomType);
                //will add the rooms to the mapData object in a sequential order then can reproduce
                //the grid given the width and height

                mapData.rooms.Add(roomData);
            }
        }
        //save the mapData object
        mapSaver.SaveGrid(mapData, "testMap");
        Debug.Log("Map saved");

    }

    public List<TileData> getTiles()
    {
        return tiles;
    }
        //methods needed for the WFC algorithm
        //1. get the cell with the lowest entropy or a random cell to start
        //2. choose and collapse the cell, remove all other possible tiles, can add a wight 
        //3. propagate the constraints through the grid
        //4. check if all cells have collapsed
        //
}


