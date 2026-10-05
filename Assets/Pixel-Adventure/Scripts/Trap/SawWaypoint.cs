using UnityEngine;

// Gắn vào các waypoint con (tag EditorOnly) của Saw để chỉnh thời gian đứng đợi trên scene
public class SawWaypoint : MonoBehaviour
{
    [Min(0)] public float waitTime;
}
