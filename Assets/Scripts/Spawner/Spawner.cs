using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<T> _pool;

    public event Action<T> Spawned;
    public event Action<T> Returned;

    protected Transform RandomizeSpawnPoint()
    {
        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.GetLength(0))];
    }

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => TakeObject(obj),
        actionOnRelease: (obj) => ReturnObject(obj),
        actionOnDestroy: (obj) => Destroy(),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    public virtual void GetObject()
    {
        if (_pool.CountActive < _poolMaxSize)
            _pool.Get();
    }

    public virtual void GetObject(T obj)
    {
        _pool.Get(out obj);
    }

    public void ReleasePool(T obj)
    {
        _pool.Release(obj);
    }

    public void TakeObject(T obj)
    {
        obj.transform.position = RandomizeSpawnPoint().position;

        if (obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;

        obj.gameObject.SetActive(true);

        Spawned?.Invoke(obj);
    }

    private void ReturnObject(T obj)
    {
        Returned?.Invoke(obj);
        obj.gameObject.SetActive(false);
    }

    protected float GetRepeatRate() => _repeatRate;

    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
