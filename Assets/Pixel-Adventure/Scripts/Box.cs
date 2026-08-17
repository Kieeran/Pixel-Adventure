using UnityEngine;

public class Box : PlacedObject
{
    [SerializeField] string boxID;
    [SerializeField] Animator animator;

    public void SetBoxID(string id) { boxID = id; }
    public string GetBoxID() { return boxID; }

    public override void UnloadObject()
    {
        throw new System.NotImplementedException();
    }
}