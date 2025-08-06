using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("GameManager initialization started.");

        // Simulate some asynchronous initialization work
        return UniTask.CompletedTask;
    }
}
