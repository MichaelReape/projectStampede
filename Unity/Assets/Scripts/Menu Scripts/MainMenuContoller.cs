using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuContoller : MonoBehaviour
{
    public GameObject ChooseOptionPanel;
    public GameObject MapSizePanel;

    public void Start()
    {
        MapSizePanel.SetActive(false);
    }
    public void NewMap()
    {
        ChooseOptionPanel.SetActive(false);
        MapSizePanel.SetActive(true);
    }
    public void LoadMap()
    {
        SceneManager.LoadScene("LoadMapTemplate");
    }
}
