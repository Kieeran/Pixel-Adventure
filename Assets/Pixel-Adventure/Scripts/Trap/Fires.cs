using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
#if UNITY_EDITOR
#endif

[ExecuteAlways]
public class Fires : PlacedObject
{
    // Khoảng cách giữa các unit được cố định trong code
    const float Spacing = 1f;
    const float UnitWidth = 1f;

    [SerializeField] FiresCollision firesCollision;
    [SerializeField] Transform unitTemplate;
    [SerializeField] Transform skin;
    [SerializeField] float knockBackForce = 1f;
    [Min(1)] public int count = 1;

    [SerializeField] List<Transform> units = new();
    [SerializeField] Transform collidedUnit;

    void OnValidate()
    {
        firesCollision = GetComponent<FiresCollision>();
        if (transform.childCount > 0) unitTemplate = transform.GetChild(0);
        if (transform.childCount > 1) skin = transform.GetChild(1);
    }

    void Update()
    {
        if (Application.isPlaying) return;
        if (unitTemplate == null || skin == null || firesCollision.col == null) return;

#if UNITY_EDITOR
        // Prefab Mode chỉ dùng để chỉnh thông số của một unit, logic design cụm chỉ chạy trên scene
        if (PrefabStageUtility.GetPrefabStage(gameObject) != null) return;
#endif

        // Template active trong prefab để dễ chỉnh, trên scene thì luôn tắt đi
        if (unitTemplate.gameObject.activeSelf) unitTemplate.gameObject.SetActive(false);

        if (skin.childCount == count) return;

        Render();
    }

    public void SetCount(int count)
    {
        this.count = Mathf.Max(1, count);
        Render();
    }

    public override void OnSpawn()
    {
        firesCollision.OnCharacterCollided += OnCharacterCollided;

        FiresData data = customData as FiresData;
        SetCount(data.count);
    }

    public override void OnDespawn()
    {
        firesCollision.OnCharacterCollided -= OnCharacterCollided;

        ClearUnits();
        count = 1;
    }

    void OnCharacterCollided(Vector2 avgContactPoint)
    {
        int collidedUnitIndex = 0;
        float closestDistance = Vector2.Distance(units[collidedUnitIndex].position, avgContactPoint);
        for (int i = 1; i < units.Count; i++)
        {
            if (Vector2.Distance(units[i].position, avgContactPoint) < closestDistance)
            {
                closestDistance = Vector2.Distance(units[i].position, avgContactPoint);
                collidedUnitIndex = i;
            }
        }
        collidedUnit = units[collidedUnitIndex];
    }

    public void Render()
    {
        ClearUnits();
        unitTemplate.gameObject.SetActive(false);

        // Unit đầu tiên nằm tại root, cụm kéo dài về phía +X local
        // Hàng dọc hay ngang là do rotation của root quyết định
        for (int i = 0; i < count; i++)
        {
            Transform unit = Instantiate(unitTemplate, skin);
            unit.gameObject.SetActive(true);
            unit.SetLocalPositionAndRotation(new Vector2(i * Spacing, 0f), Quaternion.identity);

            units.Add(unit);
        }

        // 1 collider gộp phủ cả cụm
        float length = (count - 1) * Spacing;
        firesCollision.col.size = new Vector2(length + UnitWidth, firesCollision.col.size.y);
        firesCollision.col.offset = new Vector2(length / 2f, firesCollision.col.offset.y);
    }

    void ClearUnits()
    {
        for (int i = skin.childCount - 1; i >= 0; i--)
        {
            GameObject unit = skin.GetChild(i).gameObject;

            units.Remove(unit.transform);

            if (Application.isPlaying) Destroy(unit);
            else DestroyImmediate(unit);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || collidedUnit == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(collidedUnit.position, Vector2.one);
    }
}
