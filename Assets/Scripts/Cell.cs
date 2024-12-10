using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private int xCordinate;
    private int yCordinate;
    private List<Room> posibilities = new List<Room>();
    private int entropy;
    private bool isCollapsed = false;

    //probably need to initialize this with a list of all possible rooms and then prune it

    // will need a reference to all the neighbours of this cell (up, down, left, right)
    // // Start is called before the first frame update
    // void Start()
    // {

    // }
    public Cell(int x, int y)
    {
        this.entropy = posibilities.Count;
        this.xCordinate = x;
        this.yCordinate = y;
    }

    public void collapse()
    {
        //pick the lowest entropy
        //or use a tiebreaker if the same entropy
        //collapse this tile
        //call update neighbors again
        isCollapsed = true;
    }
    //get possible rooms/tiles that this cell can be 
    public List<Room> getPosibilities()
    {
        return posibilities;
    }
    //set neighbours
    public void updatePosibilities()
    {
        //what data do i need to update the possibilities of this cell?
        //i need to know the neighbors of this cell
    }
    public int getEntropy()
    {
        return entropy;
    }
    public void updateEntropy()
    {
        this.entropy = posibilities.Count;
    }
}
