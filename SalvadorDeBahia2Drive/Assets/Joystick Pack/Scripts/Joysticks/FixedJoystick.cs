using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        //background.anchoredPosition = basePosition;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}