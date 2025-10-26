using Cysharp.Threading.Tasks;
using UnityEngine;

public class IngameManager : SingletonMono<IngameManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("GameManager initialization started.");

        // Simulate some asynchronous initialization work
        return UniTask.CompletedTask;
    }
}
