using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>, IInitializable
{
    public async UniTask InitializeAsync()
    {
        Debug.Log("DataManager initialization started.");

        await Metadata.Init();
    }
}