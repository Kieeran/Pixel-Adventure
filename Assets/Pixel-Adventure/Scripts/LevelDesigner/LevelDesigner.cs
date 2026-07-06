using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    [ContextMenu("Create a new level")]
    public void CreateLevel()
    {
        Debug.Log("Create a new level");
    }

    [ContextMenu("Save level")]
    public void SaveLevel()
    {
        Debug.Log("Save level");
    }

    [ContextMenu("Edit level")]
    public void EditLevel()
    {
        Debug.Log("Edit level");
    }
}