//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GLTFast;
//using UnityEngine.UI;

//public class GLBTest : MonoBehaviour
//{
//    public string path = "file:///C:/Users/micha/Desktop/StableDiffusion3d/stable-fast-3d/output/0/mesh.glb";
//    //public Button importButton;
//    // Start is called before the first frame update
//    void Start()
//    {
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;
//        //importButton.onClick.AddListener(() =>
//        //{
     
//        //    Debug.Log("Button clicked");
//        //});
//    }

//    private async void OnMouseDown()
//    {
//        Debug.Log("Importing GLB from " + path);
//        var temp = new GltfImport();
//        bool success = await temp.Load(path);
//        if (success)
//        {
//            Debug.Log("GLB loaded");
//            GameObject parent = new GameObject("ImportedGLB");
//            await temp.InstantiateMainSceneAsync(parent.transform);
//            //Cursor.lockState = CursorLockMode.Locked;
//            //Cursor.visible = false;
//        }
//        else
//        {
//            Debug.LogError("Failed to load GLB");
//        }
//    }


//}
using UnityEngine;
using GLTFast;
using System.Threading.Tasks;

public class GLBSpawner : MonoBehaviour
{
    public string path = "file:///C:/Users/micha/Desktop/StableDiffusion3d/stable-fast-3d/output/0/mesh.glb";

    void Start()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        //importButton.onClick.AddListener(() =>
        //        {

        //            Debug.Log("Button clicked");
        //       });
    }
    private async void OnMouseDown()
    {
        // 1. Import the GLB
        var temp = new GltfImport();
        bool success = await temp.Load(path);
        if (!success)
        {
            Debug.LogError("Failed to load GLB file.");
            return;
        }

        // 2. Create a parent GameObject to hold the model
        GameObject parent = new GameObject("ImportedGLB");

        // 3. Instantiate the main scene from the GLB
        //gltf.InstantiateMainScene(parent.transform);
        await temp.InstantiateMainSceneAsync(parent.transform);

        // 4. Add the Draggable script
        //    This will let us click and drag the object in the scene
        var draggable = parent.AddComponent<Draggable>();

        // 5. Add a BoxCollider to the parent (so OnMouseDown can fire)
        //    We'll compute a bounding box that encapsulates all child meshes
        BoxCollider boxCollider = parent.AddComponent<BoxCollider>();
        RecalculateBoundsFromChildren(parent, boxCollider);

        // Optionally reposition or scale the spawned object
        parent.transform.position = Vector3.zero;
    }

    /// <summary>
    /// Calculates a bounding box that encapsulates all MeshRenderers under 'parent'
    /// and applies it to the provided BoxCollider.
    /// </summary>
    void RecalculateBoundsFromChildren(GameObject parent, BoxCollider boxCollider)
    {
        var meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers.Length == 0)
        {
            Debug.LogWarning("No MeshRenderers found under " + parent.name);
            return;
        }

        // Start with the first renderer's bounds
        Bounds combinedBounds = meshRenderers[0].bounds;
        // Encapsulate all child mesh bounds
        for (int i = 1; i < meshRenderers.Length; i++)
        {
            combinedBounds.Encapsulate(meshRenderers[i].bounds);
        }

        // BoxCollider center is in local space, so convert from world to local
        boxCollider.center = parent.transform.InverseTransformPoint(combinedBounds.center);
        boxCollider.size = combinedBounds.size;
    }
}
