using UnityEngine;
using UnityEngine.EventSystems;

public class MobileHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsHeld { get; private set; }

    public void OnPointerDown(PointerEventData e) => IsHeld = true;
    public void OnPointerUp(PointerEventData e) => IsHeld = false;
}
