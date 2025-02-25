using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuContoller : MonoBehaviour
{
    public GameObject LoadOptionPanel;
    public GameObject ChooseOptionPanel;
    //public MapManager wfcManager;

    //onclidk will call this
    //will use scene manager to load the gamescene
    public void NewMap()
    {
        //loads teh blank map scene
        SceneManager.LoadScene("NewMapTemplate");
        Debug.Log("new map loaded");
        //MapManager.MapManagerInstance.cellSize = 8;
        //MapManager.MapManagerInstance.gridHeight = 2;
        //MapManager.MapManagerInstance.gridWidth = 2;
        //MapManager.MapManagerInstance.InitialiseGrid(2,2,8);
        //Debug.Log("grid initialised");
        //MapManager.MapManagerInstance.RunWFC();
        //Debug.Log("WFC run");
        //Debug.Log("map generated");
    }

    //onclick will call this
    //write the code later 
    //it will call another ui/canvas displaying all the saved palaces
    public void LoadMap()
    {
        SceneManager.LoadScene("LoadMapTemplate");
        //LoadOptionPanel.SetActive(true);
        //ChooseOptionPanel.SetActive(false);
        Debug.Log("load map pressed");
    }
}
