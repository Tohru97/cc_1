using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>, IInitializable
{
    public UniTask InitializeAsync()
    {
        Debug.Log("DataManager initialization started.");
        
        // Simulate some asynchronous initialization work

        return UniTask.CompletedTask;
    }
}
