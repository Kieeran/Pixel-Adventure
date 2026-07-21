using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LevelDesigner : MonoBehaviour
{
    public Level level;

    [ContextMenu("Save level")]
    public void SaveLevel()
    {
        if (level == null)
        {
            Debug.Log("Level not found");
            return;
        }
        level.levelData.placedObjectDatas.Clear();

        level.transform.position = Vector2.zero;
        foreach (Transform tf in level.placedObjectsHolder)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(tf.gameObject);

            if (TryGetAddressableKey(prefab, out var addressableKey))
            {
                level.levelData.placedObjectDatas.Add(new PlacedObjectData
                {
                    addressableKey = addressableKey,
                    position = tf.position
                });
            }
            else
            {
                Debug.Log("Không thể lấy được addressableKey từ prefab");
            }
        }
        Debug.Log("Save level complete!");
    }

    [ContextMenu("Edit level")]
    public void EditLevel()
    {
        if (level == null)
        {
            Debug.Log("Level not found");
            return;
        }

        // Clear object cũ trước khi đặt object mới load từ SO lên
        for (int i = level.placedObjectsHolder.childCount - 1; i >= 0; i--)
        {
            AddressableSpawner.DespawnInEditorImmediate(level.placedObjectsHolder.GetChild(i).gameObject);
        }

        level.transform.position = Vector2.zero;
        foreach (PlacedObjectData data in level.levelData.placedObjectDatas)
        {
            GameObject obj = AddressableSpawner.SpawnInEditorImmediate(data.addressableKey);
            obj.transform.parent = level.placedObjectsHolder;
            obj.transform.position = data.position;
        }
        Debug.Log("Reopen level level");
    }

    public bool TryGetAddressableKey(GameObject prefab, out string addressableKey)
    {
        addressableKey = string.Empty;

        if (prefab == null) return false;

        // Lấy Settings hiện tại của Addressables trong Editor
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null) return false;

        // Chuyển đổi Prefab sang GUID tài nguyên
        string assetPath = AssetDatabase.GetAssetPath(prefab);
        string guid = AssetDatabase.AssetPathToGUID(assetPath);

        // Tìm kiếm Entry của Asset này trong danh sách Addressables
        AddressableAssetEntry entry = settings.FindAssetEntry(guid);

        if (entry != null)
        {
            addressableKey = entry.address; // Đây chính là Key/Address của Prefab
            return true;
        }

        return false; // Prefab này không phải là Addressable Asset
    }
}
#endif