using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableHandler : MonoBehaviour
{
    #region Singleton
    private static AddressableHandler _instance;
    public static AddressableHandler Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion Singleton

    // Cache các handle đã Load theo Key, dùng để:
    // 1. Tránh Load trùng cùng 1 asset nhiều lần (nếu đã có thì trả thẳng từ cache).
    // 2. Có nơi lưu handle lại để Release đúng lúc rời level, tránh leak.
    private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _loadedPrefabHandles = new();

    // Load 1 prefab GỐC vào RAM (bước tốn kém, chỉ nên gọi 1 lần cho mỗi Key, ví dụ lúc vào level hoặc lúc khởi tạo Object Pool).
    // Trả về GameObject reference qua callback để nơi gọi tự Instantiate() bình thường nhiều lần trong gameplay — KHÔNG gọi lại hàm này mỗi lần spawn.
    // Nếu Key đã được load trước đó, trả về ngay từ cache thay vì load lại.
    public void LoadPrefab(string addressableKey, System.Action<GameObject> onComplete)
    {
        if (_loadedPrefabHandles.TryGetValue(addressableKey, out var cachedHandle)
            && cachedHandle.IsValid()
            && cachedHandle.Status == AsyncOperationStatus.Succeeded)
        {
            onComplete?.Invoke(cachedHandle.Result);
            return;
        }

        var handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
        handle.Completed += (h) =>
        {
            if (h.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedPrefabHandles[addressableKey] = h;
                onComplete?.Invoke(h.Result);
            }
            else
            {
                Debug.LogError($"[AddressableHandler] Không thể load prefab với Key: {addressableKey}");
                onComplete?.Invoke(null);
            }
        };
    }

    /// <summary>
    /// Giải phóng toàn bộ prefab đang cache. Gọi khi Unload xong 1 level (sau khi đã Destroy hết instance liên quan trong scene).
    /// </summary>
    public void ReleaseAll()
    {
        foreach (var handle in _loadedPrefabHandles.Values)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        _loadedPrefabHandles.Clear();
    }

    public void GetKeysByLabel(string labelName, System.Action<List<string>> onComplete = null)
    {
        Addressables.LoadResourceLocationsAsync(labelName).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> keysInLabel = new List<string>();
                foreach (var location in handle.Result)
                {
                    // location.PrimaryKey thường chứa Address Key, 
                    // nhưng nếu nó là đường dẫn, ta xử lý chuỗi nhẹ để đảm bảo luôn lấy đúng Key/Name
                    string key = location.PrimaryKey;

                    // Tránh trường hợp PrimaryKey bị dính nhãn trùng lặp hoặc GUID
                    if (!keysInLabel.Contains(key))
                    {
                        keysInLabel.Add(key);
                    }
                }

                onComplete?.Invoke(keysInLabel);
                Debug.Log($"[Runtime] Tìm thấy {keysInLabel.Count} asset mang label '{labelName}'");
            }
            else
            {
                Debug.LogError($"[Runtime] Không thể load resource locations cho label: {labelName}");
                onComplete?.Invoke(new List<string>());
            }

            // BẮT BỘC: Giải phóng handle của LoadResourceLocationsAsync để tránh Memory Leak
            Addressables.Release(handle);
        };
    }

#if UNITY_EDITOR
    public static GameObject SpawnInEditorImmediate(string addressableKey, Vector3 position = default, Quaternion rotation = default)
    {
        if (string.IsNullOrEmpty(addressableKey))
        {
            Debug.LogError("[AddressableSpawner Editor] Key bị null hoặc rỗng!");
            return null;
        }

        // 1. Lấy Settings của Addressables
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("[AddressableSpawner Editor] Không tìm thấy AddressableAssetSettings!");
            return null;
        }

        AddressableAssetEntry targetEntry = null;

        // 2. Tìm Entry quét qua toàn bộ các Groups bằng Address Key
        foreach (var group in settings.groups)
        {
            if (group == null) continue;
            foreach (var entry in group.entries)
            {
                if (entry != null && entry.address == addressableKey)
                {
                    targetEntry = entry;
                    break;
                }
            }
            if (targetEntry != null) break;
        }

        // 3. Nếu tìm thấy Entry, load Prefab Asset lên và Spawn giữ nguyên Prefab Connection
        if (targetEntry != null)
        {
            string assetPath = targetEntry.AssetPath;
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefabAsset != null)
            {
                // Spawn dạng Prefab Instance thực thụ (vẫn giữ link Prefab gốc trên Hierarchy!)
                GameObject spawnedObj = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
                if (spawnedObj != null)
                {
                    spawnedObj.transform.position = position;
                    spawnedObj.transform.rotation = rotation;

                    Undo.RegisterCreatedObjectUndo(spawnedObj, $"Spawn {addressableKey}");
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(spawnedObj.scene);

                    return spawnedObj;
                }
            }
        }

        Debug.LogError($"[AddressableSpawner Editor] Không tìm thấy Prefab Asset ứng với Key: '{addressableKey}'. Hãy kiểm tra lại tên Key trong cửa sổ Addressables Groups!");
        return null;
    }

    public static void DespawnInEditorImmediate(GameObject objToDespawn)
    {
        if (objToDespawn == null) return;

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(objToDespawn.scene);

        if (!Addressables.ReleaseInstance(objToDespawn))
        {
            // Fallback nếu object không phải do Addressables tạo ra
            DestroyImmediate(objToDespawn);
        }
    }
#endif
}
