using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;

public class CameraController : MonoBehaviour
{
    public FixedJoystick joystick;
    public RectTransform joystickArea; // área visible del joystick
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 0.2f;

    private Dictionary<int, Vector2> lastTouchPositions = new();
    private HashSet<int> ignoredFingerIds = new();
    private float rotationY = 0f;
    private float rotationX = 0f;
    void Update()
    {
        MoveWithJoystick();

        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchOverJoystickArea(touch.position))
                {
                    ignoredFingerIds.Add(id);
                }
                else
                {
                    lastTouchPositions[id] = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Moved && !ignoredFingerIds.Contains(id))
            {
                if (lastTouchPositions.ContainsKey(id))
                {
                    Vector2 delta = touch.position - lastTouchPositions[id];
                    lastTouchPositions[id] = touch.position;
                    rotationX += delta.x * rotationSpeed;
                    rotationY -= delta.y * rotationSpeed;
                    rotationY = Mathf.Clamp(rotationY, -90f, 90f);
                    Camera.main.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                ignoredFingerIds.Remove(id);
                lastTouchPositions.Remove(id);
            }
        }
    }

    void MoveWithJoystick()
    {
        Vector3 dir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        dir = cameraTransform.TransformDirection(dir);
        dir.y = 0;
        cameraTransform.position += dir * moveSpeed * Time.deltaTime;
    }

    bool IsTouchOverJoystickArea(Vector2 screenPosition)
    {
        // Convertir pantalla → UI local
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea,
            screenPosition,
            null, // si usas Canvas World, pasa la cámara aquí
            out localPoint
        );

        return joystickArea.rect.Contains(localPoint);
    }
}
