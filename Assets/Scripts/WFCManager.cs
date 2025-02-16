using System.Collections;
using System.Collections.Generic;
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
    private Vector2Int[] directions = new Vector2Int[]
    {
        //north, east, south, west
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };

    void Start()
    {
        InitialiseGrid();
        RunWFC();

    }

    //initialise the grid
    public void InitialiseGrid()
    {
        //create the grid with the given dimensions
        grid = new GridCell[gridWidth, gridHeight];
        //populate the grid with 
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new GridCell(new Vector2Int(x,y), tiles);
            }
        }
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
        return lowestEntropyCell;
    }



    public void CollapseCell(GridCell cell)
    {
        //if cell is already collapsed, return
        if (cell.chosenTile != null)
        {
            return;
        }
        //select a random tile from the possible tiles
        int randomTileIndex = Random.Range(0, cell.possibleTiles.Count);
        //set the chosen tile to the random tile
        cell.chosenTile = cell.possibleTiles[randomTileIndex];
        //remove all other possible tiles
        cell.possibleTiles.Clear();
        //set the chosen tile to the cell list of possible tiles (tidying up)
        cell.possibleTiles.Add(cell.chosenTile);
    }
    public void Propagate(GridCell cell)
    {
        //1. get the neighbours of the cell
        foreach (Vector2Int direction in directions)
        {
            //get the neighbour using the direction offset
            Vector2Int neighbourPos = cell.gridPosition + direction;
            //check if the neighbour is within the grid
            if (IsValidPosition(neighbourPos))
            {
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
                    }
                    else if (direction == directions[1])
                    {
                        //east neighbour
                        sourceWallIndex = 3;
                        neighbourWallIndex = 1;
                    }
                    else if (direction == directions[2])
                    {
                        //south neighbour
                        sourceWallIndex = 0;
                        neighbourWallIndex = 2;
                    }
                    else if (direction == directions[3])
                    {
                        //west neighbour
                        sourceWallIndex = 1;
                        neighbourWallIndex = 3;
                    }

                    //now need to filter the possible tiles based on the doors
                    foreach (TileData tile in neighbour.possibleTiles)
                    {
                        //this may be wrong, need to check
                        //right now it only checks the source cell doors but what about the walls
                        //might be a feature as it will create hallways idk

                        //if the source cell has a door in the direction
                        if (cell.chosenTile.doorPositions[sourceWallIndex] == 1)
                        {
                            //if the neighbour tile doesnt have a door in the opposite direction
                            if (tile.doorPositions[neighbourWallIndex] == 0)
                            {
                                //remove the tile from the list of possible tiles

                                //this might be a bad idea as it could cause a runtime error
                                //instead i might create a copy list and just overwrite the original at the end
                                neighbour.possibleTiles.Remove(tile);
                            }
                        }
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

        //loop through the grid and instantiate the tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //get the chosen tile
                TileData chosenTile = grid[x, y].chosenTile;
                //instantiate the tile
                GameObject tile = Instantiate(chosenTile.tilePrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
            }
        }
    }

    //methods needed for the WFC algorithm
    //1. get the cell with the lowest entropy or a random cell to start
    //2. choose and collapse the cell, remove all other possible tiles, can add a wight 
    //3. propagate the constraints through the grid
    //4. check if all cells have collapsed
    //
}
