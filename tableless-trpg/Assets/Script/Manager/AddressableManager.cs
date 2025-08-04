using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class AddressableManager : Singleton<AddressableManager>, IInitializable
{
    private Dictionary<string, AsyncOperationHandle> loadedAssets = new Dictionary<string, AsyncOperationHandle>();

    public UniTask InitializeAsync()
    {
        // 이 곳에서 필요한 공용 에셋(스프라이트 아틀라스, 기본 프리팹 등)을 미리 로드할 수 있습니다.
        // 예: await LoadAssetAsync<GameObject>("CommonUI");
        Debug.Log("AddressableManager Initialized.");
        return UniTask.CompletedTask; // 실제 비동기 작업이 있다면 해당 Task를 반환
    }

    public async UniTask<T> LoadAssetAsync<T>(string address)
    {
        if (loadedAssets.ContainsKey(address))
        {
            return (T)loadedAssets[address].Result;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        loadedAssets.Add(address, handle);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Asset failed to load: " + address);
            return default;
        }
    }

    public T LoadAssetSync<T>(string address)
    {
        if (loadedAssets.ContainsKey(address))
        {
            return (T)loadedAssets[address].Result;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        loadedAssets.Add(address, handle);
        handle.WaitForCompletion();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Asset failed to load: " + address);
            return default;
        }
    }

    public void ReleaseAsset(string address)
    {
        if (loadedAssets.TryGetValue(address, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            loadedAssets.Remove(address);
        }
        else
        {
            Debug.LogWarning("Asset not loaded, cannot release: " + address);
        }
    }
}
