using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;

public class CameraController : MonoBehaviour
{
    public Joystick joystick1, joystick2;
    public RectTransform joystickArea1, joystickArea2,sliderArea; // área visible del joystick
    public Slider speedSlider;
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float maxMoveSpeed = 50f,minMoveSpeed;
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
        // Movimiento de joystick1: adelante / atrás (Z) y izquierda / derecha (X)
        Vector3 dir1 = new Vector3(joystick1.Horizontal, 0, joystick1.Vertical);
        dir1 = cameraTransform.TransformDirection(dir1);

        // Movimiento de joystick2: izquierda / derecha (X) y arriba / abajo (Y)
        Vector3 dir2 = new Vector3(joystick2.Horizontal,0, 0);
        dir2 = cameraTransform.TransformDirection(dir2)+Vector3.up * joystick2.Vertical;
        float sliderValue = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, speedSlider.value);
        float currentSpeed = speedSlider != null ? sliderValue : moveSpeed;

        cameraTransform.position += (dir1 + dir2) * currentSpeed * Time.deltaTime;
    }

    bool IsTouchOverJoystickArea(Vector2 screenPosition)
    {
        Vector2 localPoint;
        bool overJoystick1 = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea1, screenPosition, null, out localPoint
        ) && joystickArea1.rect.Contains(localPoint);

        bool overJoystick2 = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea2, screenPosition, null, out localPoint
        ) && joystickArea2.rect.Contains(localPoint);

        bool overSlider = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            sliderArea, screenPosition, null, out localPoint
        ) && sliderArea.rect.Contains(localPoint);

        return overJoystick1 || overJoystick2 || overSlider;
    }
}
