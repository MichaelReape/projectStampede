using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    //singleton pattern to ensure only one instance of the APIManager
    //this will be accessed by the ButtonController script
    //with something like APIManager.APIInstance

    private static APIManager instance;
    private string apiEndpoint = "http://localhost:5000/generate";
    //to make sure only one api call is made at a time
    private bool isCallingAPI = false;

    public static APIManager APIInstance 
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

    //going to add a callback function to get the image from the API
    public void GetImageFromAPI(string prompt, System.Action<Sprite> onSuccess)
    {
        StartCoroutine(GetImage(prompt, onSuccess));
    }
    private IEnumerator GetImage(string prompt, System.Action<Sprite> onSuccess)
    {
        //this is where you would make the API call to get the image using the prompt
        
        if(isCallingAPI)
        {
            Debug.Log("API call already in progress");
            yield break;
        }
        isCallingAPI = true;

        //now we need to create the post request to the api endpoint
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);

        using (UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form))
        {
            //this is where the api call is made
            yield return request.SendWebRequest();

            //error handling


            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching image: " + request.error);
                Debug.Log("Server response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Response text: " + request.downloadHandler.text);
                Debug.Log("Content-Type: " + request.GetResponseHeader("Content-Type"));
                Debug.Log("Status Code: " + request.responseCode);
                Debug.Log("Data length: " + request.downloadHandler.data.Length);
                byte[] result = request.downloadHandler.data;
                //dummy texture
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                //load the image from the byte array
                //this seems to work better than the DownloadHandlerTexture.GetContent(request) 
                bool loaded = tex.LoadImage(result);
                if (loaded)
                {
                    Debug.Log("Image loaded successfully");
                    Sprite webSprite = SpriteFromTexture2D(tex);
                    onSuccess?.Invoke(webSprite);
                }
                else
                {
                    Debug.Log("Error loading image");
                }
                    // Proceed to parse as a texture
                    //this gives me an invalid cast exception

            //        Texture2D texture = DownloadHandlerTexture.GetContent(request);
            ////error handling
            //if(texture != null)
            //{
            //    Debug.Log("Image received from API");
            //    Sprite webSprite = SpriteFromTexture2D(texture);
            //    //return the sprite to the callback function
            //    onSuccess?.Invoke(webSprite);
            //}
            //else
            //{
            //    Debug.Log("Error receiving image from API");
            //}
            }
        }
        isCallingAPI = false;
    }
    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        //convert the texture to a sprite
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
