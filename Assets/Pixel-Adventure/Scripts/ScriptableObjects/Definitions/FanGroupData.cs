using UnityEngine;

[CreateAssetMenu(fileName = "FanGroupData", menuName = "Scriptable Objects/FanGroupData")]
public class FanGroupData : CustomData
{
    public string groupId;
    public Vector2 pushDirection = Vector2.zero;
}
