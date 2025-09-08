using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//class to load the map data from a file in the persistent data path
public class LoadMenuController : MonoBehaviour
{
    public MapLoader mapLoader;
    public GameObject loadButtonPrefab;
    public Transform contentParent;

    private void Start()
    {
        //sets the pause menu boolean to true so the menu wont appear when m is pressed
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(true);
        FillList();
    }
    //method to fill the list of maps
    private void FillList()
    {
        //get the list of maps from the map loader
        string[] files = mapLoader.GetMaps();
        if (files == null)
        {
            Debug.Log("No Maps found");
            return;
        }
        //cycle through the list of maps
        foreach (string file in files)
        {
            //get the name of the map cutting the .json extension
            string fileName = Path.GetFileNameWithoutExtension(file);
            //create a button for each file
            GameObject loadButton = Instantiate(loadButtonPrefab, contentParent);

            //set the text of the button to the name of the file
            loadButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = fileName;

            //add a listener to the button
            Button button = loadButton.GetComponent<Button>();
            if (button != null)
            {
                //if the button is clicked call the load map function with teh filename
                button.onClick.AddListener(() => mapLoader.LoadMap(fileName));
            }
        }
    }
}
