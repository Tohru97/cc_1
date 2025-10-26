using UnityEngine;
using UnityEngine.Pool;

public class Poolable<T> : MonoBehaviour where T : Component
{
    private IObjectPool<T> _pool;
    private T _componentToReturn;

    public void SetPool(IObjectPool<T> pool, T component)
    {
        _pool = pool;
        _componentToReturn = component;
    }

    public void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Release(_componentToReturn);
        }
        else
        {
            Debug.LogWarning("Pool is not set. Destroying object instead.", this);
            Destroy(gameObject);
        }
    }
}