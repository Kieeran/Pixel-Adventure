using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public FruitsData fruitsData;
    public BoxesData boxesData;
    public TrapData trapsData;

    public Vector3 playerPosition;
}