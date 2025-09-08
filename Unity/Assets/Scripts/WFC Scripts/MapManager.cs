using System;
using System.Collections.Generic;
using System.IO;
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
        //set number of time you want to run the WFC
        else if (numberOfRuns < 20000)
        {
            Debug.Log("Number of times run: " + numberOfRuns++);
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
    public void CollapseCell(GridCell cell)
    {
        //if cell is already collapsed, return
        if (cell.chosenTile != null)
        {
            return;
        }
        //if there are no possible tiles left, return
        if (cell.possibleTiles.Count == 0)
        {
            //error handling
            Debug.LogWarning("No possible tiles left for cell at " + cell.gridPosition);
            return;
        }
        //select a random tile from the possible tiles
        int randomTileIndex = UnityEngine.Random.Range(0, cell.possibleTiles.Count);

        //set the chosen tile to the random tile
        cell.chosenTile = cell.possibleTiles[randomTileIndex];

        //set the possible tiles list to just the chosen tile
        cell.possibleTiles = new List<TileData> { cell.chosenTile };
    }
    public void Propagate(GridCell cell)
    {
        //checks if the cell has collapsed
        if (cell.chosenTile == null)
        {
            return;
        }

        //the cells neighbours in the grid
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
                    int sourceWallIndex = 0;
                    int neighbourWallIndex = 0;

                    //directions are south, west, north, east in that order
                    if (direction == directions[2])
                    {
                        //north neighbour
                        sourceWallIndex = 2;
                        neighbourWallIndex = 0;
                    }
                    else if (direction == directions[3])
                    {
                        //east neighbour
                        sourceWallIndex = 3;
                        neighbourWallIndex = 1;
                    }
                    else if (direction == directions[0])
                    {
                        //south neighbour
                        sourceWallIndex = 0;
                        neighbourWallIndex = 2;
                    }
                    else if (direction == directions[1])
                    {
                        //west neighbour
                        sourceWallIndex = 1;
                        neighbourWallIndex = 3;
                    }

                    //filter the possible tiles based on the doors
                    List<TileData> tilesToRemove = new List<TileData>();
                    foreach (TileData tile in neighbour.possibleTiles)
                    {
                        //if the cell has a door in the direction and the neighbour doesnt have a door in the opposite direction
                        //remove the tile from the list of possible tiles
                        if (cell.chosenTile.doorPositions[sourceWallIndex] == 1 && tile.doorPositions[neighbourWallIndex] == 0)
                        {
                            //if the neighbour tile doesnt have a door in the opposite 
                            //add it to the list of tiles to remove 
                            tilesToRemove.Add(tile);
                        }
                    }
                    //remove the tiles from the list of possible tiles
                    foreach (TileData tile in tilesToRemove)
                    {
                        neighbour.possibleTiles.Remove(tile);
                    }
                }
                //recursively call the propagate method on the neighbour
                //removed this as it was causing a stack overflow exception
                // Propagate(neighbour);
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


    //check if all cells have collapsed
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
        //bfs so we need a queue
        Queue<GridCell> queue = new Queue<GridCell>();
        //need a list to store the visited cells
        List<GridCell> visited = new List<GridCell>();
        //start from the 0,0 cell
        queue.Enqueue(grid[0, 0]);
        //add the 0,0 cell to the visited list
        visited.Add(grid[0, 0]);
        //queue all the connected cells and visit them until we reach the end
        while (queue.Count > 0)
        {
            //take the first cell from the queue
            GridCell currentCell = queue.Dequeue();
            //find the connected cells add them to the queue
            for (int i = 0; i < directions.Length; i++)
            {
                if (currentCell.chosenTile.doorPositions[i] == 1)
                {
                    //get the neighbour cell
                    Vector2Int neighbourPos = currentCell.gridPosition + directions[i];
                    //check if the neighbour is within the grid
                    if (IsValidPosition(neighbourPos))
                    {
                        //Debug.Log("the neighbour is valid");
                        GridCell neighbour = grid[neighbourPos.x, neighbourPos.y];
                        //check if the neighbour has been visited
                        if (!visited.Contains(neighbour) && isDoorConnected(currentCell, neighbour, i))
                        {
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

    public void InstantiateTiles()
    {
        //loop through the grid and instantiate the tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //grab the cell
                GridCell cell = grid[x, y];
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
                            Sprite webSprite = APIManager.APIInstance.SpriteFromTexture2D(tex);

                            if (i < buttonControllers.Length)
                            {
                                //grabs the image component from the button controller through the canvaa
                                Image image = buttonControllers[i].promptCanvasController.image;
                                image.sprite = webSprite;
                            }
                        }
                    }
                }
            }
        }
        //lock the cursor and hide it, allow the player to move, sets the menu flag to false
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.PlayerMovementInstance.CanMove = true;
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(false);
    }

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
                //create a new roomData object and load it with the cell data
                RoomData roomData = new RoomData();
                roomData.x = x;
                roomData.y = y;
                roomData.roomType = cell.chosenTile.tileName;

                //add the image paths
                roomData.imagePaths = cell.imagePaths;
                //add the roomData object to the mapData object
                mapData.rooms.Add(roomData);
            }
        }
        //add the object data to the mapData object
        mapData.objects = ObjectSaver.ObjectSaverInstance.objectDataList;
        //save the mapData object
        mapSaver.SaveGrid(mapData, mapName);
    }

    public void ReconstructGrid(MapData map)
    {
        //rebuilt the grid from the map data
        gridHeight = map.height;
        gridWidth = map.width;
        cellSize = map.cellSize;
        grid = new GridCell[gridWidth, gridHeight];
        //get the tiles from the tile manager
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
        //instantiate the tiles
        InstantiateTiles();
    }
}