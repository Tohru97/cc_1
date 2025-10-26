using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    protected virtual async UniTaskVoid Awake()
    {
        await LoadAsync();
        AppManager.Instance.OnSceneLoad(this);
    }

    public abstract eScene GetCurrentSceneType();
    public abstract UniTask LoadAsync();
}
