using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public string SaveImage(byte[] imageBytes, string file)
    {
        //for organisation we will save the images in a folder called "Images"
        string imageDirectory = Path.Combine(Application.persistentDataPath, "Saves" , "Images");
        Debug.Log("Image Directory: " + imageDirectory);
        //check if the directory exists
        if (!Directory.Exists(imageDirectory))
        {
            //if it doesn't exist create it
            Directory.CreateDirectory(imageDirectory);
        }

        //create teh file path
        string fileName = file + ".png";
        string filePath = Path.Combine(imageDirectory, fileName);

        //check if the file already exists
        int counter = 1;
        while (File.Exists(filePath))
        {
            //if it does add counter to the file name
            fileName = file + counter + ".png";
            Debug.Log("New file name: " + fileName);
            filePath = Path.Combine(imageDirectory, fileName);
            counter++;
        }

        //write the image to the file
        File.WriteAllBytes(filePath, imageBytes);
        Debug.Log("Image saved to: " + filePath);

        //return the file path to be used in the save file
        return filePath;
    }
}
