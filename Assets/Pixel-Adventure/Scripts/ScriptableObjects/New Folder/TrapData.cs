using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrapData", menuName = "ScriptableObjects/Trapdata")]
public class TrapData : ScriptableObject
{
    // TrapID
    public int trampolineID;
    public int blockID;
    public int fanID;

    // Trap position
    public List<Vector3> trampolinePosition;
    public List<Vector3> blockPosition;
    public List<Vector3> fanPosition;
}