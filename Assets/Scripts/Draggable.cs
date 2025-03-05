using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;
    private float zDistanceToCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        //Distance from the camera to the object
        zDistanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        //Convert the object's position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        //Mouse position in screen space
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        //Calculate offset so the object doesn't snap to the cursor
        offset = transform.position - mouseWorldPos;
        //need to add funtionality so the object rotates as i move

    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        //Current mouse position in screen space, at the same Z distance
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        //Move the object to follow the mouse, maintaining the offset
        transform.position = mouseWorldPos + offset;

        //rotate the object
        if(Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up, 0.5f);
        }

        //scale the object
        //get the scroll wheel input


        //if (Input.GetAxis("Mouse ScrollWheel") > 0.1f )
        //{
        //    Debug.Log("scroll");
        //    //transform.localScale += new Vector3(scroll, scroll, scroll);
        //}
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Increase or decrease scale uniformly
            Vector3 newScale = transform.localScale + Vector3.one * scroll * 0.5f;
            // Prevent negative or zero scale
            newScale = Vector3.Max(newScale, Vector3.one * 0.01f);
            transform.localScale = newScale;
        }

        //testing deleting the object
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ObjectSaver.ObjectSaverInstance.DeleteObject(gameObject.name);
            Destroy(gameObject);

        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        //on mouse up ill log the position, rotation adn scale of the object to the object saver data structure
        ObjectSaver.ObjectSaverInstance.UpdateData(gameObject.name, transform.position, transform.rotation, transform.localScale);
    }

    //method for rotating the object when mousedown
    //will need to have a keybind to rotate the object clockwise and anticlockwise


    //method for scaling the object when mousedown
    //will use the scroll wheel to scale the object up and down
    //will need to set a minimum scale and a maximum scale

    //method for deleting the object when mousedown
    //will need to have a keybind to delete the object
}
