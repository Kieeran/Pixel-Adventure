using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanGroupSequencer : MonoBehaviour
{
    [SerializeField] float activeDuration = 2f;
    [SerializeField] float gapDuration = 0.5f;

    Dictionary<string, List<Fan>> fanGroups;
    List<string> groupIds;
    Coroutine sequenceRoutine;

    void Start()
    {
        LevelManager.Instance.CurrentLevelLoaded += OnCurrentLevelLoaded;
    }

    void OnDisable()
    {
        StopSequence();
    }

    void OnCurrentLevelLoaded()
    {
        StopSequence();

        fanGroups = new();
        groupIds = new();
        List<PlacedObject> activeObjects = PoolManager.Instance.activeObjects;
        foreach (var obj in activeObjects)
        {
            if (obj.TryGetComponent<Fan>(out var fan))
            {
                string groupId = (fan.customData as FanGroupData).groupId;
                if (!fanGroups.ContainsKey(groupId))
                {
                    fanGroups[groupId] = new();
                    groupIds.Add(groupId);
                }
                fanGroups[groupId].Add(fan);
            }
        }

        if (groupIds.Count > 0)
        {
            sequenceRoutine = StartCoroutine(RunFanGroups());
        }
    }

    IEnumerator RunFanGroups()
    {
        while (true)
        {
            foreach (var groupId in groupIds)
            {
                List<Fan> fans = fanGroups[groupId];

                foreach (var fan in fans)
                {
                    fan.Activate();
                }

                yield return new WaitForSeconds(activeDuration);

                foreach (var fan in fans)
                {
                    fan.Deactivate();
                }

                yield return new WaitForSeconds(gapDuration);
            }
        }
    }

    void StopSequence()
    {
        if (sequenceRoutine != null)
        {
            StopCoroutine(sequenceRoutine);
            sequenceRoutine = null;
        }
    }
}
