using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class Chains : MonoBehaviour
{
    [SerializeField] Transform prefabChain;
    [SerializeField] Transform chainHolder;
    [SerializeField] Transform waypointsRoot;
    public float spacing = 0.5f;
    public List<Vector2> waypoints = new();
    public Vector2 rootPos = Vector2.zero;

    readonly List<Transform> spawnedChains = new();
    readonly List<Vector2> lastBakedPositions = new();
    Transform lastPrefabChain;

    public void SetProperties(List<Vector2> waypoints, Vector2 rootPos, float spacing)
    {
        this.waypoints.Clear();
        this.waypoints = new List<Vector2>(waypoints);
        this.rootPos = rootPos;
        this.spacing = spacing;

        transform.localPosition = rootPos;

        if (!Application.isPlaying)
        {
            RestoreWaypointMarkers();
            lastPrefabChain = prefabChain;
        }

        Render();
    }

    void RestoreWaypointMarkers()
    {
        if (waypointsRoot == null) return;

        while (waypointsRoot.childCount > waypoints.Count)
        {
            DestroyImmediate(waypointsRoot.GetChild(waypointsRoot.childCount - 1).gameObject);
        }

        while (waypointsRoot.childCount < waypoints.Count)
        {
            new GameObject("Waypoint").transform.SetParent(waypointsRoot);
        }

        lastBakedPositions.Clear();
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform marker = waypointsRoot.GetChild(i);
            marker.name = $"Waypoint_{i}";
            marker.position = transform.TransformPoint(waypoints[i]);
            lastBakedPositions.Add(waypoints[i]);
        }
    }

    void OnValidate()
    {
        if (transform.childCount > 0) prefabChain = transform.GetChild(0);
        if (transform.childCount > 1) chainHolder = transform.GetChild(1);
        if (transform.childCount > 2) waypointsRoot = transform.GetChild(2);

        rootPos = transform.localPosition;
    }

    void Update()
    {
        if (Application.isPlaying) return;
        if (waypointsRoot == null) return;
        if (!HasChanges()) return;

        Bake();

        lastPrefabChain = prefabChain;
    }

    bool HasChanges()
    {
        if (prefabChain != lastPrefabChain) return true;
        if (waypointsRoot.childCount != lastBakedPositions.Count) return true;

        for (int i = 0; i < waypointsRoot.childCount; i++)
        {
            Vector2 markerPosition = transform.InverseTransformPoint(waypointsRoot.GetChild(i).position);
            if (markerPosition != lastBakedPositions[i]) return true;
        }

        return false;
    }

    [ContextMenu("Bake")]
    public void Bake()
    {
        waypoints.Clear();
        lastBakedPositions.Clear();

        if (waypointsRoot == null) return;

        for (int i = 0; i < waypointsRoot.childCount; i++)
        {
            Vector2 markerPosition = transform.InverseTransformPoint(waypointsRoot.GetChild(i).position);
            waypoints.Add(markerPosition);
            lastBakedPositions.Add(markerPosition);
        }

        Render();
    }

    public void Render()
    {
        ClearChains();

        if (prefabChain == null || spacing <= 0f || waypoints.Count < 2) return;

        Vector2 currentPoint = waypoints[0];
        SpawnChainAt(currentPoint);

        float distanceUntilNextChain = spacing;
        int segmentIndex = 0;

        while (segmentIndex < waypoints.Count - 1)
        {
            Vector2 segmentEnd = waypoints[segmentIndex + 1];
            float distanceToSegmentEnd = Vector2.Distance(currentPoint, segmentEnd);

            if (distanceToSegmentEnd < distanceUntilNextChain)
            {
                distanceUntilNextChain -= distanceToSegmentEnd;
                currentPoint = segmentEnd;
                segmentIndex++;
                continue;
            }

            Vector2 direction = (segmentEnd - currentPoint).normalized;
            currentPoint += direction * distanceUntilNextChain;
            SpawnChainAt(currentPoint);
            distanceUntilNextChain = spacing;
        }
    }

    void SpawnChainAt(Vector2 localPosition)
    {
        Transform chain = Instantiate(prefabChain, chainHolder);
        chain.gameObject.SetActive(true);
        chain.localPosition = localPosition;
        spawnedChains.Add(chain);
    }

    void ClearChains()
    {
        for (int i = spawnedChains.Count - 1; i >= 0; i--)
        {
            Transform chain = spawnedChains[i];
            if (chain == null) continue;

            if (Application.isPlaying) Destroy(chain.gameObject);
            else DestroyImmediate(chain.gameObject);
        }

        spawnedChains.Clear();
    }
}
