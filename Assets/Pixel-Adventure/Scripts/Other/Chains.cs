using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Rải các chain (mắt xích) dọc theo đường gấp khúc đi qua các waypoint.
///
/// Cấu trúc object con (theo thứ tự, OnValidate tự gán):
///   con 0 = prefabChain    : chain mẫu, được Instantiate ra nhiều bản
///   con 1 = chainHolder    : chứa các chain được spawn
///   con 2 = waypointHolder : chứa các marker (waypoint) để kéo thả khi design level
///
/// Luồng dữ liệu:
///   - Khi design (Editor): kéo marker -> cập nhật waypoints -> Render lại (xem Update).
///   - Khi load level / play: SetProperties(data) -> Render (không dùng marker).
/// </summary>
[ExecuteAlways]
public class Chains : MonoBehaviour
{
    // Chặn trường hợp spacing quá nhỏ (hoặc đường quá dài) làm spawn quá nhiều, treo editor
    const int MaxChains = 2000;

    [SerializeField] Transform prefabChain;
    [SerializeField] Transform chainHolder;
    [SerializeField] Transform waypointHolder;

    public SpriteRenderer chainSkin;

    // Khoảng cách giữa 2 chain, đo DỌC THEO ĐƯỜNG ĐI (path length), không phải khoảng cách thẳng.
    // Nên ở góc cua, khoảng cách thẳng giữa 2 chain sẽ nhỏ hơn spacing (góc càng gắt càng nhỏ).
    public float spacing = 0.5f;

    // Vị trí các waypoint ở local space của object này. Đây là dữ liệu chính của level.
    public List<Vector2> waypoints = new();
    public Vector2 rootPos = Vector2.zero;

    // Khởi tạo các properties cần thiết cho chains khi:
    // - Muốn khôi phục chains trở lại để tiếp tục design level
    // - Muốn đặt chains lên level khi đang play
    public void SetProperties(List<Vector2> waypoints, Vector2 rootPos, float spacing, Sprite chainSkin)
    {
        // Copy sang list mới để không bị dùng chung list với nơi gọi
        this.waypoints = new List<Vector2>(waypoints);
        this.rootPos = rootPos;
        this.spacing = spacing;
        this.chainSkin.sprite = chainSkin;

        transform.localPosition = rootPos;

        // Nếu khôi phục chains trở lại để tiếp tục design level thì khôi phục cả các wp để design
#if UNITY_EDITOR
        if (!Application.isPlaying) SyncMarkersFromWaypoints();
#endif

        Render();
    }

    // Rải chain dọc theo waypoints.
    //
    // Hình dung có một "cây bút" đi dọc đường gấp khúc, cứ đi đủ `spacing` thì đặt một chain.
    //   currentPoint           : vị trí hiện tại của cây bút
    //   distanceUntilNextChain : quãng cây bút còn phải đi nữa thì mới đặt chain tiếp theo
    //   segmentIndex           : đoạn đang đi, là đoạn từ waypoints[segmentIndex] -> waypoints[segmentIndex + 1]
    public void Render()
    {
        // Xoá chain cũ trước khi vẽ lại
        ClearChains();

        if (prefabChain == null || chainHolder == null) return;
        if (spacing <= 0f || waypoints.Count < 2) return;

        // Bắt đầu spawn ở wp 0
        Vector2 currentPoint = waypoints[0];
        SpawnChainAt(currentPoint);

        float distanceUntilNextChain = spacing;
        int segmentIndex = 0;
        int spawned = 1;

        while (segmentIndex < waypoints.Count - 1 && spawned < MaxChains)
        {
            Vector2 segmentEnd = waypoints[segmentIndex + 1];
            float distanceToSegmentEnd = Vector2.Distance(currentPoint, segmentEnd);

            // Nhánh 1: đoạn còn lại ngắn hơn quãng cần đi -> không đủ chỗ đặt chain trong đoạn này.
            // Cây bút đi hết đoạn tới waypoint kế, rồi sang đoạn tiếp theo.
            // Quãng đã đi được trừ vào distanceUntilNextChain, phần còn thiếu mang sang đoạn sau.
            // Nhờ vậy ở góc cua: (chain trước -> wp) + (wp -> chain sau) = đúng 1 spacing.
            // Nhánh này cũng chạy khi còn dư cuối đoạn (vd đoạn dài 1.0, spacing 0.4 -> dư 0.2 mang sang đoạn sau).
            if (distanceToSegmentEnd < distanceUntilNextChain)
            {
                // chưa đủ khoảng cách -> sang đoạn kế, giữ phần dư để spacing đều qua góc cua
                distanceUntilNextChain -= distanceToSegmentEnd;
                currentPoint = segmentEnd;
                segmentIndex++;
                continue;
            }

            // Nhánh 2: đoạn còn đủ dài -> tiến đúng distanceUntilNextChain theo hướng đoạn, đặt chain,
            // rồi reset để chain kế tiếp cách đúng 1 spacing.
            // Lưu ý distanceUntilNextChain có thể < spacing ở chain đầu tiên sau góc cua (do phần dư mang sang).
            Vector2 direction = (segmentEnd - currentPoint).normalized;
            currentPoint += direction * distanceUntilNextChain;
            SpawnChainAt(currentPoint);
            spawned++;
            distanceUntilNextChain = spacing;
        }
    }

    void SpawnChainAt(Vector2 localPosition)
    {
        Transform chain = Instantiate(prefabChain, chainHolder);
        // prefabChain (con 0) thường đang tắt để không hiện ra, nên bản copy phải bật lên
        chain.gameObject.SetActive(true);
        chain.localPosition = localPosition;
    }

    // Xoá toàn bộ con của chainHolder (không cần list riêng -> hết lỗi "dây ma" sau reload/recompile).
    // Trước đây dùng list spawnedChains (không serialize) nên sau reload list rỗng, chain cũ nằm lại trong scene.
    // Lưu ý: Chains.prefab cần được save lại để chainHolder được serialize đúng.
    void ClearChains()
    {
        if (chainHolder == null) return;

        // Duyệt ngược vì xoá con sẽ làm đổi childCount / index
        for (int i = chainHolder.childCount - 1; i >= 0; i--)
        {
            GameObject child = chainHolder.GetChild(i).gameObject;
            // Destroy chỉ xoá ở cuối frame (dùng khi Play), DestroyImmediate xoá ngay (dùng ở Editor)
            if (Application.isPlaying) Destroy(child);
            else DestroyImmediate(child);
        }
    }

#if UNITY_EDITOR
    // Giá trị spacing của lần render gần nhất, để biết khi nào người dùng đổi spacing trong Inspector
    float lastSpacing;

    // Gán reference theo thứ tự con (xem mô tả ở đầu class) và đồng bộ rootPos với vị trí thực
    void OnValidate()
    {
        if (transform.childCount > 0) prefabChain = transform.GetChild(0);
        if (transform.childCount > 1) chainHolder = transform.GetChild(1);
        if (transform.childCount > 2) waypointHolder = transform.GetChild(2);

        chainSkin = prefabChain.GetComponent<SpriteRenderer>();
        rootPos = transform.localPosition;
    }

    // Kéo marker / đổi spacing / thêm xoá marker -> tự render lại.
    // Không render trực tiếp trong OnValidate vì Instantiate/Destroy bị cấm ở đó.
    void Update()
    {
        if (Application.isPlaying || waypointHolder == null) return;
        if (!HasChanges()) return;

        RefreshFromMarkers();
    }

    // So sánh trạng thái hiện tại (marker + spacing) với lần render gần nhất.
    // Không cần lưu list vị trí riêng vì waypoints luôn đồng bộ với marker sau mỗi lần Refresh.
    // Lưu ý: đổi prefabChain (thay con 0 bằng object khác) KHÔNG tự render lại, cần kéo marker hoặc đổi spacing.
    bool HasChanges()
    {
        // spacing mà khác lastSpacing thì đã có sự thay đổi
        if (!Mathf.Approximately(spacing, lastSpacing)) return true;
        // số wp thực tế ở waypointHolder khác số wp ở waypoints thì vừa thêm wp
        if (waypointHolder.childCount != waypoints.Count) return true;

        // Detect thay đổi vị trí wp
        // (Vector2 != so sánh gần đúng, nên sai số float khi đổi qua lại world/local không gây render liên tục)
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (GetMarkerLocalPosition(i) != waypoints[i]) return true;
        }

        return false;
    }

    // marker (scene) -> waypoints (data) -> render
    void RefreshFromMarkers()
    {
        waypoints.Clear();
        for (int i = 0; i < waypointHolder.childCount; i++)
        {
            waypoints.Add(GetMarkerLocalPosition(i));
        }

        lastSpacing = spacing;

        Render();
    }

    // waypoints (data) -> marker (scene)
    void SyncMarkersFromWaypoints()
    {
        if (waypointHolder == null) return;

        // Tăng giảm số waypoint sao cho bằng với số waypoint được lưu ở dạng data
        while (waypointHolder.childCount > waypoints.Count)
        {
            DestroyImmediate(waypointHolder.GetChild(waypointHolder.childCount - 1).gameObject);
        }

        while (waypointHolder.childCount < waypoints.Count)
        {
            CreateMarker(waypointHolder.childCount);
        }

        // Set các giá trị từ data vào các waypoint
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform marker = waypointHolder.GetChild(i);
            marker.name = $"Waypoint_{i}";
            marker.position = transform.TransformPoint(waypoints[i]);
        }

        // Đặt mốc để Update không tưởng là có thay đổi và render lại lần nữa
        lastSpacing = spacing;
    }

    // Vị trí marker ở local space của object này (waypoints cũng lưu ở local space).
    // InverseTransformPoint trả Vector3, gán sang Vector2 sẽ bỏ z.
    Vector2 GetMarkerLocalPosition(int index)
    {
        return transform.InverseTransformPoint(waypointHolder.GetChild(index).position);
    }

    // Tag EditorOnly: Unity loại các object có tag này khỏi build
    Transform CreateMarker(int index)
    {
        GameObject marker = new($"Waypoint_{index}") { tag = "EditorOnly" };
        marker.transform.SetParent(waypointHolder, false);
        return marker.transform;
    }

    [ContextMenu("Add waypoint")]
    void AddWaypoint()
    {
        if (waypointHolder == null) return;

        int index = waypointHolder.childCount;
        Transform marker = CreateMarker(index);
        marker.localPosition = Vector2.zero;

        // Cho phép Ctrl+Z để hoàn tác việc thêm waypoint
        Undo.RegisterCreatedObjectUndo(marker.gameObject, "Add waypoint");
    }
#endif
}