using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//class to save the images to a file in the persistent data path
public class ImageSaver : MonoBehaviour
{
    private static ImageSaver instance;
    public static ImageSaver ImageSaverInstance
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

    //method to save the image to a file
    public string SaveImage(byte[] imageBytes, string file)
    {
        //Images to be saved in a folder called Images in the Saves folder
        string imageDirectory = Path.Combine(Application.persistentDataPath, "Saves", "Images");

        //check if the directory exists
        if (!Directory.Exists(imageDirectory))
        {
            //if it doesn't exist create it
            Directory.CreateDirectory(imageDirectory);
        }

        //create teh file path string
        string fileName = file + ".png";
        string filePath = Path.Combine(imageDirectory, fileName);

        //check if the file already exists in the saves folder we will append a number
        int counter = 1;
        while (File.Exists(filePath))
        {
            //if it does add counter to the file name
            fileName = file + counter + ".png";
            //log the name to the console
            Debug.Log("New file name: " + fileName);
            //create the new file path
            filePath = Path.Combine(imageDirectory, fileName);
            //increment the counter
            counter++;
        }

        //write the image to the file
        File.WriteAllBytes(filePath, imageBytes);
        Debug.Log("Image saved to: " + filePath);

        //return the file path to be used in the save file
        return filePath;
    }
}
