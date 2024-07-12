using UnityEngine;

public class Factory<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    protected T Prefab => _prefab;

    public virtual T Create()
    {
        T t = Instantiate(_prefab);

        return t;
    }   
    
    public virtual T Create(Transform transform)
    {
        T t = Instantiate(_prefab, transform);

        return t;
    }
}
