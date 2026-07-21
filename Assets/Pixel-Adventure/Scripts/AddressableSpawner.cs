using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSpawner : MonoBehaviour
{
    #region Singleton
    private static AddressableSpawner _instance;
    public static AddressableSpawner Instance => _instance;

    [SerializeField] private List<GameObject> activeSpawnedObjects = new();

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

    /// <summary>
    /// Hàm Spawn chuẩn RUNTIME (Bất đồng bộ - An toàn cho WebGL, Mobile, PC).
    /// Sử dụng Callback Action để lấy về GameObject sau khi load xong.
    /// </summary>
    public void Spawn(string addressableKey, System.Action<GameObject> onComplete = null)
    {
        Addressables.InstantiateAsync(addressableKey).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject spawnedObj = handle.Result;
                activeSpawnedObjects.Add(spawnedObj);
                onComplete?.Invoke(spawnedObj);
            }
            else
            {
                Debug.LogError($"[Spawner] Không thể load hoặc instantiate asset với Key: {addressableKey}");
                onComplete?.Invoke(null);
            }
        };
    }

    public void Despawn(GameObject objToDespawn)
    {
        if (objToDespawn == null) return;

        if (activeSpawnedObjects.Contains(objToDespawn))
        {
            activeSpawnedObjects.Remove(objToDespawn);
        }

        Addressables.ReleaseInstance(objToDespawn);
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
