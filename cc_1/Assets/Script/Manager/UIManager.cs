using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("UIManager initialization started.");

        // Simulate some asynchronous initialization work
        return UniTask.CompletedTask;
    }
}
