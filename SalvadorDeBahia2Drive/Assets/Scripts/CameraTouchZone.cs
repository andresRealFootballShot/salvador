using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchZone : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public bool isDragging = false;
    private Vector2 lastPosition;

    public float sensitivity = 0.5f;
    public CharacterMovement CharacterMovement; // Arrástralo desde el Inspector

    public void OnPointerDown(PointerEventData eventData)
    {
        
        lastPosition = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("aa");
        Vector2 delta = eventData.position - lastPosition;
        lastPosition = eventData.position;

        if (CharacterMovement != null && isDragging)
        {
            CharacterMovement.RotateCamera(delta.x, delta.y);
        }
    }
}
