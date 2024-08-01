using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxesData", menuName = "ScriptableObjects/ItemData/BoxesData")]
public class BoxesData : ScriptableObject
{
    // Box's ID
    public int box1ID;
    public int box2ID;
    public int box3ID;

    // Fruit drop number
    public int fruitDropNum_Box1 = 2;
    public int fruitDropNum_Box2 = 3;
    public int fruitDropNum_Box3 = 5;

    // Box's Positions
    public List<Vector3> box1Position;
    public List<Vector3> box2Position;
    public List<Vector3> box3Position;
}