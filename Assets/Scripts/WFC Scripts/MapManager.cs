using System;
// using System.Collections;
using System.Collections.Generic;
// using System.Data;
using System.IO;
// using System.Linq;
// using TMPro.EditorUtilities;
// using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    //grid dimensions can be set
    public int gridWidth;
    public int gridHeight;
    //all rooms have the same size at the minute
    public int cellSize;
    public string mapName;
    //list of all the tile prefabs
    public List<TileData> tiles;
    //2d array to hold the grid
    public GridCell[,] grid;
    public TileManager tileManager;
    private int numberOfRuns = 0;
    public MapSaver mapSaver;

    //directions for the neighbours
    public Vector2Int[] directions = new Vector2Int[]
    {
        //south, west, north, east
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(1,0)
    };

    //singleton pattern to ensure only one instance of the WFCManager
    public static MapManager MapManagerInstance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //Method to run the WFC
    public void RunWFC(int width, int height)
    {
        //set the grid with the given dimensions
        InitialiseGrid(width, height);

        Debug.Log("Running WFC");
        //boolean to check if all cells have collapsed
        bool allCollapsed = false;
        //boolean to ensure a valid path exists
        bool pathExists = false;

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
        //check if a path exists
        pathExists = CheckPath();
        if (pathExists)
        {
            Debug.Log("Number of times run: " + numberOfRuns++);
            Debug.Log("Path exists: " + pathExists);
            Debug.Log("Instantiating tiles");
            InstantiateTiles();
        }

        //after testing I got stackoverflow exception at 7986 runs os limited to 7500
        else if (numberOfRuns < 7500)
        {
            Debug.Log("Number of times run: " + numberOfRuns++);
            Debug.Log("Path does not exist");
            RunWFC(width, height);
        }
    }

    //initialise the grid
    public void InitialiseGrid(int width, int height)
    {
        //set the grid dimensions
        gridHeight = height;
        gridWidth = width;
        //cell size could be set here but is default set to 10 in the application
        //cellSize = size;

        //create the grid with the given dimensions
        grid = new GridCell[gridWidth, gridHeight];
        //populate the grid with cells that have all possible tiles available
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //pass the location and the list of tiles to the grid cell
                grid[x, y] = new GridCell(new Vector2Int(x, y), tiles);
            }
        }

        Debug.Log("Grid initialised");
        //here we will remove the boundary tiles
        RemoveBoundaryTiles(grid);
    }

    //Removes the illegal tiles on the boundary of the grid
    public void RemoveBoundaryTiles(GridCell[,] grid)
    {
        Debug.Log("Removing boundary tiles");

        foreach (GridCell cell in grid)
        {
            List<TileData> validTiles = new List<TileData>();

            foreach (TileData tile in cell.possibleTiles)
            {
                bool isValid = true;

                // Left boundary (West doors not allowed)
                if (cell.gridPosition.x == 0 && tile.doorPositions[1] == 1)
                {
                    isValid = false;
                }

                // Right boundary (East doors not allowed)
                if (cell.gridPosition.x == gridWidth - 1 && tile.doorPositions[3] == 1)
                {
                    isValid = false;
                }

                // Bottom boundary (South doors not allowed)
                if (cell.gridPosition.y == 0 && tile.doorPositions[0] == 1)
                {
                    isValid = false;
                }

                // Top boundary (North doors not allowed)
                if (cell.gridPosition.y == gridHeight - 1 && tile.doorPositions[2] == 1)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    validTiles.Add(tile);
                }
            }
            cell.possibleTiles = validTiles;
        }
    }


    //returns a GridCell object with the lowest entropy
    public GridCell GetLowestEntropy()
    {
        //we cycle through the grid and get the cell with the lowest amount of possiblities
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

    //this is a method to remove the illegal cells just based on the grid boundaries
    //will be run before the collapse method


    public void CollapseCell(GridCell cell)
    {
        //i want to create an empty list instead of clearing and adding to the original
        //if cell is already collapsed, return
        if (cell.chosenTile != null)
        {
            return;
        }

        //Debug.Log( " THE NUMBER OF POSSIBLE TILES ARE:  "+cell.possibleTiles.Count);
        if (cell.possibleTiles.Count == 0)
        {
            Debug.LogWarning("No possible tiles left for cell at " + cell.gridPosition);
            //causing the game to crash
            return;
        }
        //select a random tile from the possible tiles
        int randomTileIndex = UnityEngine.Random.Range(0, cell.possibleTiles.Count);
        //seems to tend to 0
        //print the random Tile index
        //Debug.Log("the random tile index is " + randomTileIndex);
        //print the random tile
        //Debug.Log("the random tile is " + cell.possibleTiles[randomTileIndex].tileName);
        //set the chosen tile to the random tile
        //Debug.Log(randomTileIndex);
        //Debug.Log(cell.possibleTiles.Count);

        cell.chosenTile = cell.possibleTiles[randomTileIndex];


        //print the chosen tile      
        //Debug.Log("The cell is collapsed, the chosen tile is " + cell.chosenTile.tileName + " at the grid position " + cell.gridPosition.x + " , " + cell.gridPosition.y);
        //Debug.Log("The cells doors are " + cell.chosenTile.doorPositions[0] + " " + cell.chosenTile.doorPositions[1] + " " + cell.chosenTile.doorPositions[2] + " " + cell.chosenTile.doorPositions[3]);
        //remove all other possible tiles
        //cell.possibleTiles.Clear();
        //set the chosen tile to the cell list of possible tiles (tidying up)
        //cell.possibleTiles.Add(cell.chosenTile);

        //add the chosen tile to the temp list
        //possibleTilesTemp.Add(cell.chosenTile);
        //set the possible tiles to the temp list
        cell.possibleTiles = new List<TileData> { cell.chosenTile };
        //possibleTilesTemp.Clear();
    }
    public void Propagate(GridCell cell)
    {
        if (cell.chosenTile == null)
        {
            //Debug.LogWarning("Tried to propagate from a cell that isn't collapsed: " + cell.gridPosition);
            return;
        }
        //Debug.Log("Propagating constraints " + cell.gridPosition);
        //1. get the neighbours of the cell
        foreach (Vector2Int direction in directions)
        {
            //print the direction
            //Debug.Log("the direction we are looking is " + direction);
            //get the neighbour using the direction offset
            Vector2Int neighbourPos = cell.gridPosition + direction;
            //check if the neighbour is within the grid
            if (IsValidPosition(neighbourPos))
            {
                //print here
                //Debug.Log("the neighbour position is " + neighbourPos + " and is valid");
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
                    //Debug.Log("the neighbour has not collapsed");
                    int sourceWallIndex = 0;
                    int neighbourWallIndex = 0;

                    //little confusing because the order for doors is south, west, north, east
                    //but for directions it is north, east, south, west
                    //might change later
                    if (direction == directions[2])
                    {
                        //north neighbour
                        //their south door should be the same as our north door
                        sourceWallIndex = 2;
                        neighbourWallIndex = 0;
                        //Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        //Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[3])
                    {
                        //east neighbour
                        sourceWallIndex = 3;
                        neighbourWallIndex = 1;
                        //Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        //Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[0])
                    {
                        //south neighbour
                        sourceWallIndex = 0;
                        neighbourWallIndex = 2;
                        //Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        //Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
                    }
                    else if (direction == directions[1])
                    {
                        //west neighbour
                        sourceWallIndex = 1;
                        neighbourWallIndex = 3;
                        //Debug.Log("sourceWallIndex: " + sourceWallIndex);
                        //Debug.Log("neighbourWallIndex: " + neighbourWallIndex);
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
                        if (cell.chosenTile.doorPositions[sourceWallIndex] == 1 && tile.doorPositions[neighbourWallIndex] == 0)
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


    //method to check if there is a path from 0,0 to all other cells, through the connected door positions
    //for example a cell with doorPositions[0,1,0,0] will connect with [0,0,1,0] or [0,0,1,1] or [0,1,1,0] etc
    //if not then reset the grid and run the WFC again
    //we will use a breadth first search to check if there is a path
    public bool CheckPath()
    {
        Debug.Log("Checking path");
        //bfs so we need a queue
        Queue<GridCell> queue = new Queue<GridCell>();
        //need a list to store the visited cells
        List<GridCell> visited = new List<GridCell>();
        //start from the 0,0 cell
        queue.Enqueue(grid[0, 0]);
        //add the 0,0 cell to the visited list
        visited.Add(grid[0, 0]);
        //we are going to queue all the connected cells and visit them until we reach the end
        while (queue.Count > 0)
        {
            //take the first cell from the queue
            GridCell currentCell = queue.Dequeue();
            //find the connected cells add them to the queue
            for (int i = 0; i < directions.Length; i++)
            {
                if (currentCell.chosenTile.doorPositions[i] == 1)
                {
                    //Debug.Log(i);
                    //Debug.Log(directions[i]);
                    //get the neighbour cell
                    Vector2Int neighbourPos = currentCell.gridPosition + directions[i];
                    //Debug.Log("the neighbour position is " + neighbourPos);
                    //check if the neighbour is within the grid
                    if (IsValidPosition(neighbourPos))
                    {
                        //Debug.Log("the neighbour is valid");
                        GridCell neighbour = grid[neighbourPos.x, neighbourPos.y];
                        //check if the neighbour has been visited
                        if (!visited.Contains(neighbour) && isDoorConnected(currentCell, neighbour, i))
                        {
                            //bug where doors face walls probably getting counted as valid so need to check
                            //Debug.Log("Definitely connected");
                            //Debug.Log("the neighbour is " + neighbour.gridPosition);
                            //add the neighbour to the queue
                            queue.Enqueue(neighbour);
                            //add the neighbour to the visited list
                            visited.Add(neighbour);
                        }
                    }
                }
            }

        }
        //check if all cells have been visited
        if (visited.Count == gridWidth * gridHeight)
        {
            Debug.Log("Path exists");
            //print the visited cells
            //foreach (GridCell cell in visited)
            //{
            //    Debug.Log("Visited cell at " + cell.gridPosition);
            //}
            return true;
        }
        else
        {
            Debug.Log("Path does not exist");
            return false;
        }
    }

    public bool isDoorConnected(GridCell cell, GridCell neighbour, int directionIndex)
    {
        //should be the opposite of the direction
        //so index +2
        int neighbourDoorDirectionIndex = (directionIndex + 2) % 4;

        //return true if bnoth have a 1 at teh door position
        return cell.chosenTile.doorPositions[directionIndex] == 1 && neighbour.chosenTile.doorPositions[neighbourDoorDirectionIndex] == 1;

    }
    //check if the door is connected to a wall
    //if the door is connected to a wall then the cell is not


    //last thing to do is to create a method to instantiate the tiles
    //this will be done after the grid has been collapsed
    public void InstantiateTiles()
    {
        Debug.Log("Instantiating tiles");
        //this might be wrong, need to check
        //is instantiating the tiles with a gap between them
        //maybe need to change the cell size to 1

        //loop through the grid and instantiate the tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //print the cell 
                //Debug.Log("the cell is " + grid[x, y].chosenTile.tilePrefab.name);
                //maybe do a safety check here
                //
                //
                //
                GridCell cell = grid[x, y];
                //print the chosen tile
                //Debug.Log("the chosen tile is " + grid[x, y].chosenTile.tileName + " at position " + x + ", " + y);
                //get the chosen tile
                TileData chosenTile = cell.chosenTile;
                //instantiate the tile
                GameObject tile = Instantiate(chosenTile.tilePrefab, new Vector3(x * cellSize, 0, y * cellSize), chosenTile.tilePrefab.transform.rotation);

                //sets the grid position of the button objects in the tile
                ButtonController[] buttonControllers = tile.GetComponentsInChildren<ButtonController>();
                foreach (ButtonController buttonController in buttonControllers)
                {
                    buttonController.setGrid(x, y);
                }

                //code to load the saved images to the corresponding buttons objects
                for (int i = 0; i < cell.imagePaths.Length; i++)
                {
                    String imagePath = cell.imagePaths[i];
                    //check if the image exists
                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                        //load the image from the byte array
                        if (tex.LoadImage(imageBytes))
                        {
                            //probably need to refactor this
                            //dont like calling the APIManager for this
                            //maybe should keep it dumb
                            Sprite webSprite = APIManager.APIInstance.SpriteFromTexture2D(tex);
                            //buttonControllers[i].SetImage(webSprite);

                            //not quite sure how to do this
                            if (i < buttonControllers.Length)
                            {
                                //Image image = buttonControllers[i].GetComponent<Image>();
                                //grabs the image component from the button controller through the canvaa
                                Image image = buttonControllers[i].promptCanvasController.image;
                                image.sprite = webSprite;
                            }
                        }
                    }
                }
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
        //Debug.Log("Tiles instantiated");
    }

    public void saveMap()
    {
        //this is where i build the map to save
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

                //will add the rooms to the mapData object in a sequential order then can reproduce
                //the grid given the width and height
                //add the image paths
                roomData.imagePaths = cell.imagePaths;
                mapData.rooms.Add(roomData);
            }
        }
        mapData.objects = ObjectSaver.ObjectSaverInstance.objectDataList;
        //save the mapData object
        mapSaver.SaveGrid(mapData, mapName);
        Debug.Log("Map saved");
    }

    public void ReconstructGrid(MapData map)
    {
        //this will be fed the map data from teh json file and will reconstruct the grid array
        gridHeight = map.height;
        Debug.Log("the grid height is " + gridHeight);
        gridWidth = map.width;
        Debug.Log("the grid width is " + gridWidth);
        cellSize = map.cellSize;
        Debug.Log("the cell size is " + cellSize);
        //cellSize = map.cellSize;
        grid = new GridCell[gridWidth, gridHeight];
        tiles = tileManager.GetTileDatas();
        //populate the grid with the room data
        foreach (RoomData room in map.rooms)
        {
            //create a new grid cell
            GridCell cell = new GridCell(new Vector2Int(room.x, room.y), tiles);
            //set the chosen tile
            foreach (TileData tile in tiles)
            {
                if (tile.tileName.Equals(room.roomType))
                {
                    cell.chosenTile = tile;
                }
            }
            //set the image paths
            cell.imagePaths = room.imagePaths;
            //add the cell to the grid
            grid[room.x, room.y] = cell;
        }
        Debug.Log("Grid reconstructed");
        //instantiate the tiles
        InstantiateTiles();
    }
}