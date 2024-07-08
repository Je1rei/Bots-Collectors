using System;
using UnityEngine;

public class Storage<T> : MonoBehaviour where T : Resource<T>
{
    private int _countResources;

    public event Action<int> Changed;

    public void IncreaseResource()
    {
        _countResources++;
        Changed?.Invoke(_countResources);
    }
}
