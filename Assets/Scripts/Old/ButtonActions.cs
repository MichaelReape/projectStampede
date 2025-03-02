//using UnityEngine;

//public class ButtonActions : MonoBehaviour
//{
//    public LoadImageFromInternet imageLoader; // Assign this in the Inspector
//    public string imageUrl = ""; // Set your default URL
//    public GameObject menuCanvas;
//    public GameObject imageObject;

//    public void SpawnImage()
//    {
//        if (imageLoader != null)
//        {
//            imageLoader.DownloadAndDisplayImage(imageUrl);
//        }
//        else
//        {
//            Debug.LogWarning("Image Loader is not assigned.");
//        }

//        menuCanvas.SetActive(false);
//        // Make the image visible
//        var imageComponent = imageObject.GetComponent<UnityEngine.UI.Image>();
//        if (imageComponent != null)
//        {
//            Color newColor = imageComponent.color;
//            // Set alpha to 1
//            newColor.a = 1;
//            imageComponent.color = newColor;
//        }
//    }
//}