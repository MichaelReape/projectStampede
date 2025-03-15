using UnityEngine;

public class ObjectFunctionality : MonoBehaviour
{
    private Camera mainCamera;
    private bool isHolding;
    private Vector3 offset;
    private float zDistanceToCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isHolding = true;

        //distance from the camera to the object
        zDistanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        //cnvert the object's position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        //mouse position in screen space
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        //calculate offset so the object doesn't snap to the cursor
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseDrag()
    {
        if (!isHolding)
        {
            return;
        }

        //current mouse position in screen space, at the same Z distance
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        //Move the object to follow the mouse, maintaining the offset
        transform.position = mouseWorldPos + offset;

        //rotate the object
        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up, 0.5f);
        }

        //scale the object
        //get the scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            //increase or decrease scale uniformly
            Vector3 newScale = transform.localScale + Vector3.one * scroll * 0.5f;
            //prevent negative or zero scale
            newScale = Vector3.Max(newScale, Vector3.one * 0.01f);
            //set the new scale
            transform.localScale = newScale;
        }

        //testing deleting the object
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //removes from storage
            ObjectSaver.ObjectSaverInstance.DeleteObject(gameObject.name);
            //removes from scene
            Destroy(gameObject);
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
        //on mouse up log the position, rotation adn scale of the object to the object saver data structure
        ObjectSaver.ObjectSaverInstance.UpdateData(gameObject.name, transform.position, transform.rotation, transform.localScale);
    }
}
