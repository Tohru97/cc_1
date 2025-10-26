using Cysharp.Threading.Tasks;
using UnityEngine;

public class ObjectPoolManager : SingletonMono<ObjectPoolManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("ObjectPoolManager Initialized.");
        return UniTask.CompletedTask;
    }
}
