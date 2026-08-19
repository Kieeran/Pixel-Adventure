using System;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public string Id { get; set; }

    public virtual void OnSpawn() { }
    public virtual void OnDespawn() { }
}
