using System;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public string Id { get; set; }
    public CustomData customData;

    public virtual void OnSpawn() { }
    public virtual void OnDespawn() { }
    public void EnableObject()
    {
        gameObject.SetActive(true);
        OnSpawn();
    }
}
