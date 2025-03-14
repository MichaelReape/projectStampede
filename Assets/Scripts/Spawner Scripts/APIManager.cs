using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class APIManager : MonoBehaviour
{

    private static APIManager instance;
    private string apiEndpoint;
    //to make sure only one api call is made at a time
    public bool isCallingAPI = false;
    private string localPath;
    private int gridx;
    private int gridy;
    private int buttonIndex;

    //singleton pattern for easy access to the APIManager
    public static APIManager APIInstance
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

    public void GetImageFromAPI(string prompt, int x, int y, int index, System.Action<Sprite> onSuccess)
    {
        //set the grid location and button index from where the call is made
        gridx = x;
        gridy = y;
        buttonIndex = index;
        StartCoroutine(GetImage(prompt, onSuccess));
    }
    private IEnumerator GetImage(string prompt, System.Action<Sprite> onSuccess)
    {
        //api endpoint
        apiEndpoint = "http://localhost:5000/generateImage";
        //only 1 call at a time
        if (isCallingAPI)
        {
            Debug.Log("API call already in progress");
            yield break;
        }
        //sets flag
        isCallingAPI = true;

        //form to send the prompt
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);
        //using the unity web request
        using (UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form))
        {
            //start the request
            yield return request.SendWebRequest();

            //error handling
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching image: " + request.error);
                Debug.Log("Server response: " + request.downloadHandler.text);
            }
            else
            {
                //get the image bytes as a byte array
                byte[] result = request.downloadHandler.data;
                //dummy texture
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                //load the image from the byte array
                //this seems to work better than the DownloadHandlerTexture.GetContent(request) 
                bool loaded = tex.LoadImage(result);

                if (loaded)
                {
                    //convert the texture to a sprite
                    Sprite webSprite = SpriteFromTexture2D(tex);
                    //store the image file in the persistent data path
                    string savedImagePath = ImageSaver.ImageSaverInstance.SaveImage(result, prompt);
                    //saves teh image path to the specific room/grid cell for the specific button
                    MapManager.MapManagerInstance.grid[gridx, gridy].imagePaths[buttonIndex] = savedImagePath;
                    //MapManager.MapManagerInstance.saveMap();

                    //return the sprite to the callback function
                    onSuccess?.Invoke(webSprite);
                }
                else
                {
                    Debug.Log("Error loading image");
                }
            }
        }
        //reset the flag
        isCallingAPI = false;
    }

    public string Get3DObjectFromAPI(string prompt, System.Action<string> onSuccess)
    {
        StartCoroutine(GetObject(prompt, onSuccess));
        localPath += prompt + ".glb";
        //return the path to the object in the persistent data path
        //this will be used by the object loader to instantiate the object
        return localPath;
    }
    private IEnumerator GetObject(string prompt, System.Action<string> onSuccess)
    {
        //only 1 call at a time
        if (isCallingAPI)
        {
            Debug.Log("API call already in progress");
            yield break;
        }
        //set flag
        isCallingAPI = true;
        //api endpoint
        apiEndpoint = "http://localhost:5000/generate3D";

        //form to send the prompt
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);
        //using the unity web request
        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiEndpoint, form))
        {
            //start  request
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                //file name in storage is the prompt
                string filename = prompt + ".glb";
                //ensure the directory structure exists
                localPath = Path.Combine(Application.persistentDataPath, "Saves", "Objects");
                string dir = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                //file path
                string filePath = Path.Combine(localPath, filename);
                //write the downloaded bytes to file
                File.WriteAllBytes(filePath, webRequest.downloadHandler.data);
                Debug.Log("GLB file saved to: " + localPath);
                //return the path to the object
                onSuccess?.Invoke(localPath);
            }
            else
            {
                //error handling
                Debug.LogError($"Error downloading GLB: {webRequest.error}");
            }
        }
        //reset flag
        isCallingAPI = false;
    }

    //helper method to convert the texture to a sprite
    public Sprite SpriteFromTexture2D(Texture2D texture)
    {
        //convert the texture to a sprite
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
