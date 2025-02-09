using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuContoller : MonoBehaviour
{
    public GameObject LoadOptionPanel;
    public GameObject ChooseOptionPanel;

    //onclidk will call this
    //will use scene manager to load the gamescene
    public void NewMap()
    {
        //loads teh blank map scene
        SceneManager.LoadScene("NewMapTemplate");
        Debug.Log("new map loaded");
    }

    //onclick will call this
    //write the code later 
    //it will call another ui/canvas displaying all the saved palaces
    public void LoadMap()
    {
        LoadOptionPanel.SetActive(true);
        ChooseOptionPanel.SetActive(false);
        Debug.Log("load map pressed");
    }
}
