using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using System.IO;

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
    public async void LoadObject(string path, string prompt)
    {
        //load the object from the path using the gltf importer
        var temp = new GltfImport();
        bool success = await temp.Load(path);

        if (success)
        {
            Debug.Log("GLB loaded");
            GameObject parent = new GameObject("ImportedGLB");
            //set the name of the object to the prompt for easy identification
            parent.name = prompt;
            await temp.InstantiateMainSceneAsync(parent.transform);
            //add the functionality to the object
            var draggable = parent.AddComponent<ObjectFunctionality>();

            Vector3 position = PlayerMovement.PlayerMovementInstance.transform.position;
            //spawns overhead
            position.y += 1.5f;
            parent.transform.position = position;
            //add a box collider
            BoxCollider boxCollider = parent.AddComponent<BoxCollider>();
            RecalculateBoundsFromChildren(parent, boxCollider);
            //update the object data structure for saving
            ObjectSaver.ObjectSaverInstance.UpdateData(prompt, parent.transform.position, parent.transform.rotation, parent.transform.localScale);
        }
        else
        {
            //error handle
            Debug.LogError("Failed to load GLB");
        }
    }

    private void RecalculateBoundsFromChildren(GameObject parent, BoxCollider boxCollider)
    {
        var meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        //start with the first renderer's bounds
        Bounds combinedBounds = meshRenderers[0].bounds;
        //encapsulate all child mesh bounds
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            combinedBounds.Encapsulate(meshRenderers[i].bounds);
        }
        //BoxCollider center is in local space, so convert from world to local
        boxCollider.center = parent.transform.InverseTransformPoint(combinedBounds.center);
        boxCollider.size = combinedBounds.size;
    }

    //object loader from json
    public async void LoadObjectFromSave(ObjectData objectData)
    {
        //load the object from the json data
        string localPath = Path.Combine(Application.persistentDataPath, "Saves", "Objects");
        string filePath = Path.Combine(localPath, objectData.name + ".glb");
        var temp = new GltfImport();
        bool success = await temp.Load(filePath);
        if (success)
        {
            //create the object
            GameObject parent = new GameObject("ImportedGLB");
            //set the name of the object to the json name
            parent.name = objectData.name;
            Debug.Log("Parent name: " + parent.name);
            //instantiate the object
            await temp.InstantiateMainSceneAsync(parent.transform);
            //add the draggable script
            var draggable = parent.AddComponent<ObjectFunctionality>();
            //set the object to the parameters in json
            parent.transform.position = objectData.position;
            parent.transform.rotation = objectData.rotation;
            parent.transform.localScale = objectData.scale;
            //add a box collider
            BoxCollider boxCollider = parent.AddComponent<BoxCollider>();
            RecalculateBoundsFromChildren(parent, boxCollider);
            ObjectSaver.ObjectSaverInstance.UpdateData(objectData.name, objectData.position, objectData.rotation, objectData.scale);
        }
    }
}
