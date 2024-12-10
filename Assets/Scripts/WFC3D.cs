using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFC3D : MonoBehaviour
{
    //demensions of the grid
    public int height;
    public int width;
    public int mapSize;
    //cell size scale
    //the cells will be maybe 10 * 10 blocks or 5*10 for certain rooms etc
    //array of tiles
    private Cell[,] grid;
    // Start is called before the first frame update

    //initialize the grid
    // have a designated start point
    //update the list of neighbors based on the first tile
    //pick the lowest entropy
    //or use a tiebreaker if the same entropy
    //collapse this tile
    //call update neighbors again

    //each cell will have a certain number of tiles that it is possible for it to be
    //each cell will have a certain number of neighbors, the type of tiles these
    // neighbors can be will be updated

    void runWFC()
    {
        //initialize the grid
        mapSize = width * height;

        //may need to change the size of the cells
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell(x, y);
                //creates the grid of cells and gives them an x and y coordinate
            }
        }
        //so here ive initialized the grid full of new cell objects 
        //each cell onject will have a list of its neighbours possible rooms in it, in the class that is
        //and will have a collapse method and probably a pick lowest entropy method
        //or i can group them together

        //all the room class should have is a list of allowed neighbors for each room and a room type


        //maybe the list of neighbours should be a 2d array of [cells, possible room]
        //initialize the list of neighbors
        //i think i implement this from the room class
        //or its just empty list now then i choose a room to start with and it will be updated
        List<Room> neighbours = new List<Room>();
        //initialize the start point
        //it depends where i want the front door
        //width/2 puts it in the middle of the x axis and 0 height puts it at the bottom
        Cell start = new Cell(width / 2, 0);

        //update the list of neighbors based on the first tile
        updateNeighbours(start, neighbours);
        //pick the lowest entropy
        Cell lowestEntropy = pickLowestEntropy(neighbours);
        //collapse this tile
        collapseCell(lowestEntropy);
        //call update neighbors again
        updateNeighbours(lowestEntropy, neighbours);
    }

    void updateNeighbours(Cell current, List<Room> neighbours)
    {
        //each room will have a list of possible neighbors
        //if this is called before any is collapsed then the list will be all possible 
        //maybe we colllapse this with a certain probability or weight so that we have
        //room for randomness but in general get a more consitent layout
    }
    //maybe this should pick the room with the lowest entropy for the whole grid 
    //instead of just the neighbors not sure

    Cell pickLowestEntropy(List<Room> neighbours)
    {
        //pick the room with the lowest entropy
        return null;
    }
    void collapseCell(Cell current)
    {
        //collapse the tile
    }
}
//can use a reference to the triangle project creadted in web dev in year 3, game loop instead of a while loop 


