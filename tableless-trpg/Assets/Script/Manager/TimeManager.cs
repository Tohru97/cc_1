using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("TimeManager initialization started.");

        // Simulate some asynchronous initialization work
        return UniTask.CompletedTask;
    }
}
