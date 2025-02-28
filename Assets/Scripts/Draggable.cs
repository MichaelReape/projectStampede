using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;
    private float zDistanceToCamera;

    void Start()
    {
        // Make sure you have a Main Camera tagged as "MainCamera" in your scene
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Distance from the camera to the object
        zDistanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Convert the object's position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        // Mouse position in screen space
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // Calculate offset so the object doesn't snap to the cursor
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // Current mouse position in screen space, at the same Z distance
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // Move the object to follow the mouse, maintaining the offset
        transform.position = mouseWorldPos + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
