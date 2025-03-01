using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;

public class ObjectLoader : MonoBehaviour
{
    // make a singleton
    public static ObjectLoader instance;
    public static ObjectLoader ObjectLoaderInstance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        //singleton pattern to ensure only one instance of the APIManager      
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public async void LoadObject(string path)
    {
        //doesnt have the name of the object
        Debug.Log("Importing GLB from " + path);
        var temp = new GltfImport();
        bool success = await temp.Load(path);
        if (success)
        {
            Debug.Log("GLB loaded");
            GameObject parent = new GameObject("ImportedGLB");
            await temp.InstantiateMainSceneAsync(parent.transform);
        }
        else
        {
            Debug.LogError("Failed to load GLB");
        }
    }

}
