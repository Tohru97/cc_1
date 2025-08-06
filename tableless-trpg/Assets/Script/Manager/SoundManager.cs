using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("SoundManager initialization started.");

        // Simulate some asynchronous initialization work
        return UniTask.CompletedTask;
    }
}
