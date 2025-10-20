using UnityEngine;
using UnityEngine.Pool;

public class ComponentPool<T> where T : MonoBehaviour
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    public IObjectPool<T> Pool { get; }

    public ComponentPool(GameObject prefab, Transform parent, int initialCapacity = 10, int maxSize = 20)
    {
        _prefab = prefab;
        _parent = parent;

        Pool = new ObjectPool<T>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            initialCapacity,
            maxSize);

        WarmUp(initialCapacity);
    }

    private void WarmUp(int count)
    {
        var warmUpArray = new T[count];
        for (int i = 0; i < count; i++)
        {
            warmUpArray[i] = Get();
        }
        for (int i = 0; i < count; i++)
        {
            Release(warmUpArray[i]);
        }
    }

    private T CreatePooledItem()
    {
        GameObject instance = Object.Instantiate(_prefab, _parent);
        T component = instance.GetComponent<T>();
        
        if (component == null)
        {
            Debug.LogError($"Prefab '{_prefab.name}' is missing component of type '{typeof(T).Name}'");
        }
        
        // 생성된 오브젝트가 스스로 풀에 돌아올 수 있도록 Poolable 컴포넌트 추가 및 설정
        var poolable = instance.AddComponent<Poolable<T>>();
        poolable.SetPool(Pool, component);

        return component;
    }

    public T Get() => Pool.Get();

    private void OnTakeFromPool(T component)
    {
        component.gameObject.SetActive(true);
    }

    public void Release(T component) => Pool.Release(component);

    private void OnReturnedToPool(T component)
    {
        component.gameObject.SetActive(false);
    }

    // 4. 풀이 가득 찼거나 삭제될 때 객체를 파괴하는 함수
    private void OnDestroyPoolObject(T component)
    {
        Object.Destroy(component.gameObject);
    }
}
