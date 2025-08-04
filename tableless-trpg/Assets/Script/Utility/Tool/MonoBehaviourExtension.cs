using UnityEngine;

public abstract class MonoBehaviourExtension : MonoBehaviour
{
    // Abstract methods
    public abstract void Init();
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    public abstract void Show();
    public abstract void Hide();
    public abstract void Reset();
}