using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    //object sver singleton
    private static ObjectSaver instance;

    //serialised data structure to store the object data
    
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
                //print the data to the console
                //Debug.Log("Object Name: " + objectDataList[i].name);
                //Debug.Log("Object Position: " + objectDataList[i].position);
                //Debug.Log("Object Rotation: " + objectDataList[i].rotation);
                //Debug.Log("Object Scale: " + objectDataList[i].scale);
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
}
