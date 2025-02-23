using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    private Transform player;
    [SerializeField] public Image image;
    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    { 
        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= interactionDistance)
        {
            Debug.Log("Click");
            APIManager.APIInstance.GetImageFromAPI("A big,fat, orange cat smoking a bong", (Sprite result) =>
            {
                image.sprite = result;
            });
        }
        
    }
    private void OnMouseUp()
    {
        Debug.Log("Clack");
    }
}
