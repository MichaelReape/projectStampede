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
        //this will be a blank map
        SceneManager.LoadScene("GameScene");
        Debug.Log("new map pressed");
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
