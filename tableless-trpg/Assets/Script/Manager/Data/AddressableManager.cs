using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.IO;

public class AddressableManager : Singleton<AddressableManager>, IInitializable
{
    // 로드된 에셋 핸들을 주소 기준으로 관리
    private readonly Dictionary<string, AsyncOperationHandle> _loadedAssetHandles = new Dictionary<string, AsyncOperationHandle>();
    // 모든 에셋의 이름을 키로, 주소를 값으로 저장
    public readonly Dictionary<string, string> AssetAddresses = new Dictionary<string, string>();

    public async UniTask InitializeAsync()
    {
        // 각 라벨별로 에셋 주소를 로드하여 단일 Dictionary에 모두 저장
        await LoadAssetAddressesByLabelAsync("prefab");
        await LoadAssetAddressesByLabelAsync("image");
        await LoadAssetAddressesByLabelAsync("table");

        Debug.Log("AddressableManager Initialized.");
    }

    private async UniTask LoadAssetAddressesByLabelAsync(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach (var location in locations)
        {
            var key = Path.GetFileNameWithoutExtension(location.PrimaryKey);
            if (!AssetAddresses.ContainsKey(key))
            {
                AssetAddresses.Add(key, location.PrimaryKey);
            }
            else
            {
                Debug.LogWarning($"Asset with key '{key}' already exists. Skipping duplicate address: {location.PrimaryKey}");
            }
        }
    }

    public async UniTask<T> LoadAssetAsync<T>(string name) where T : Object
    {
        if (!AssetAddresses.TryGetValue(name, out var address))
        {
            Debug.LogError($"Asset name not found: {name}");
            return default;
        }

        if (_loadedAssetHandles.TryGetValue(address, out var loadedHandle))
        {
            return (T)loadedHandle.Result;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        _loadedAssetHandles.Add(address, handle);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Asset failed to load with name: {name} at address: {address}");
            Debug.LogError(handle.OperationException);
            _loadedAssetHandles.Remove(address); // 실패 시 핸들 제거
            return default;
        }
    }

    public T LoadAssetSync<T>(string name) where T : Object
    { 
        if (!AssetAddresses.TryGetValue(name, out var address))
        {
            Debug.LogError($"Asset name not found: {name}");
            return default;
        }

        if (_loadedAssetHandles.TryGetValue(address, out var loadedHandle))
        {
            return (T)loadedHandle.Result;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        _loadedAssetHandles.Add(address, handle);
        handle.WaitForCompletion();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Asset failed to load with name: {name} at address: {address}");
            _loadedAssetHandles.Remove(address); // 실패 시 핸들 제거
            return default;
        }
    }

    public void ReleaseAsset(string name)
    {
        if (!AssetAddresses.TryGetValue(name, out var address))
        {
            Debug.LogWarning($"Cannot release asset. Name not found: {name}");
            return;
        }

        if (_loadedAssetHandles.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            _loadedAssetHandles.Remove(address);
        }
        else
        {
            Debug.LogWarning($"Asset not loaded, cannot release: {name}");
        }
    }
}
