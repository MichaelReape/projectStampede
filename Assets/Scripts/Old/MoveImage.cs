using UnityEngine;
using UnityEngine.EventSystems;

public class MoveImage : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // This method detects the start of the click
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // This method can initialize things for when dragging starts
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the image as the player drags it
        // Convert the screen point to a world point in the canvas
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var worldPos);
        rectTransform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // This method can be used to finalize placement, if needed
    }
}