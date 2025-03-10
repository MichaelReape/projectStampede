using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    //object sver singleton
    //singleton pattern to ensure only one instance of the object saver
    //useful for calling the instance from other scripts
    private static ObjectSaver instance;

    //list of the object data
    //name, position, rotation, scale
    public List<ObjectData> objectDataList = new List<ObjectData>();
    public static ObjectSaver ObjectSaverInstance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        //singleton pattern to ensure only one instance of the obejct saver     
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //method to add the object data to the data structure
    public void UpdateData(string name, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        //check if the object is already in the list
        for (int i = 0; i < objectDataList.Count; i++)
        {
            if (objectDataList[i].name.Equals(name))
            {
                //if it is update the data
                objectDataList[i].position = position;
                objectDataList[i].rotation = rotation;
                objectDataList[i].scale = scale;
                return;
            }
        }
        //if not create
        ObjectData objectData = new ObjectData();
        objectData.name = name;
        objectData.position = position;
        objectData.rotation = rotation;
        objectData.scale = scale;
        objectDataList.Add(objectData);
    }
    //function to remove an object from the list
    public void DeleteObject(string name)
    {
        //cycle through the list to find the object
        for (int i = 0; i < objectDataList.Count; i++)
        {
            //if the object is found remove it
            if (objectDataList[i].name.Equals(name))
            {
                objectDataList.RemoveAt(i);
                break;
            }
        }
        //remove it from the persistent data if it exists
        string path = Application.persistentDataPath + "/Saves/Objects/" + name + ".glb";

        if (System.IO.File.Exists(path))
        {
            Debug.Log("Deleting file " + path);
            System.IO.File.Delete(path);
        }
    }
}
