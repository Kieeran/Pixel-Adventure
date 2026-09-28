using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class Spikes : PlacedObject
{
    // Khoảng cách giữa các unit được cố định trong code
    const float Spacing = 1f;
    const float UnitWidth = 1f;

    [SerializeField] SpikesCollision spikesCollision;
    [SerializeField] Transform unitTemplate;
    [SerializeField] Transform skin;
    [SerializeField] BoxCollider2D col;
    [Min(1)] public int count = 1;

    void OnValidate()
    {
        spikesCollision = GetComponent<SpikesCollision>();
        if (transform.childCount > 0) unitTemplate = transform.GetChild(0);
        if (transform.childCount > 1) skin = transform.GetChild(1);
        col = GetComponentInChildren<BoxCollider2D>(true);
    }

    void Update()
    {
        if (Application.isPlaying) return;
        if (unitTemplate == null || skin == null || col == null) return;

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
        spikesCollision.OnCharacterCollided += OnCharacterCollided;

        SpikesData data = customData as SpikesData;
        SetCount(data.count);
    }

    public override void OnDespawn()
    {
        spikesCollision.OnCharacterCollided -= OnCharacterCollided;

        ClearUnits();
        count = 1;
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
        }

        // 1 collider gộp phủ cả cụm
        float length = (count - 1) * Spacing;
        col.size = new Vector2(length + UnitWidth, col.size.y);
        col.offset = new Vector2(length / 2f, col.offset.y);
    }

    void ClearUnits()
    {
        for (int i = skin.childCount - 1; i >= 0; i--)
        {
            GameObject unit = skin.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(unit);
            else DestroyImmediate(unit);
        }
    }

    void OnCharacterCollided()
    {
        PlayerController.Instance.playerPhysic.ReceiveDamage();
    }
}
