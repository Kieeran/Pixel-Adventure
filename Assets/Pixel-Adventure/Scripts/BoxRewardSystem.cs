using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxRewardSystem : MonoBehaviour
{
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    void Start()
    {
        EventChannel.Instance.OnBoxHit += OnBoxHit;
        EventChannel.Instance.OnBoxBroken += OnBoxBroken;
    }

    void OnBoxHit(Box box)
    {
        if (box.boxRewardDataWhenHit == null) return;

        Distribute(box, box.boxRewardDataWhenHit);
    }

    void OnBoxBroken(Box box)
    {
        if (box.boxRewardDataWhenBroken == null) return;

        Distribute(box, box.boxRewardDataWhenBroken);
    }

    void Distribute(Box box, BoxRewardData boxRewardData)
    {
        string label = boxRewardData.label;
        AddressableHandler.Instance.GetKeysByLabel(label, keys =>
        {
            if (keys == null || keys.Count == 0)
            {
                Debug.Log("Can't get keys with label " + label);
                return;
            }

            keys.Shuffle();
            List<string> fruitIdsToSpawn = keys.Take(boxRewardData.amount).ToList();

            foreach (string id in fruitIdsToSpawn)
            {
                PlacedObject obj = PoolManager.Instance.GetByID(id);
                if (obj.TryGetComponent<Fruit>(out var fruit))
                {
                    LevelManager.Instance.currentLevel.AddPlacedObject(obj);

                    fruit.ApplyGravity();
                    fruit.transform.position = box.transform.position + Vector3.up * 0.5f;
                    Launch(fruit.rb);
                    StartCoroutine(EnableSolidWhenClear(fruit, box));
                }
                else
                {
                    Debug.LogError("Label is not 'Fruit'");
                    PoolManager.Instance.Return(obj);
                    return;
                }
            }
        });
    }

    void Launch(Rigidbody2D rb)
    {
        float angle = Random.value < 0.5f
            ? Random.Range(165f, 195f)
            : Random.Range(-15f, 15f);

        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new(Mathf.Cos(rad), Mathf.Sin(rad));

        rb.AddForce(dir * Random.Range(minForce, maxForce), ForceMode2D.Impulse);
    }

    IEnumerator EnableSolidWhenClear(Fruit fruit, Box box)
    {
        Transform fruitTf = fruit.transform;
        Transform boxTf = box.transform;
        float fruitRadius = fruit.fruitCollision.cachedColliderRadius;
        float boxRadius = box.boxCollision.cachedColliderRadius;

        while (true)
        {
            float dist = Vector2.Distance(fruitTf.position, boxTf.position);

            if (dist > fruitRadius + boxRadius)
            {
                fruit.fruitCollision.surfaceCollider.enabled = true;
                yield break;
            }

            yield return null;
        }
    }
}
