using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadMenuController : MonoBehaviour
{
    public MapLoader mapLoader;
    public GameObject loadButtonPrefab;
    public Transform contentParent;

    private void Start()
    {
        PauseMenuController.PMCInstance.SetIsPauseMenuOpen(true);
        FillList();
    }
    private void FillList()
    {
        string[] files = mapLoader.GetFiles();
        if (files == null)
        {
            Debug.Log("No files found");
            return;
        }
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            //create a button for each file
            GameObject loadButton = Instantiate(loadButtonPrefab, contentParent);

            //set the text of the button to the name of the file
            loadButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = fileName;
            //maybe remove json here somehow

            //add a listener to the button
            Button button = loadButton.GetComponent<Button>();
            if(button != null)
            {
                button.onClick.AddListener(() => mapLoader.LoadMap(fileName));
            }
        }
    }
}
