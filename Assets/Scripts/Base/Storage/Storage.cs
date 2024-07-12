using System;
using UnityEngine;

public class Storage<T> : MonoBehaviour where T : Resource<T>
{
    private int _countResources;

    public event Action<int> Changed;
    
    public int CountResources => _countResources;

    public void IncreaseResource()
    {
        _countResources++;
        Changed?.Invoke(_countResources);
    }

    public void DecreaseResource(int value) 
    {
        if((_countResources - value) >= 0) 
        {
            _countResources -= value;
            Changed?.Invoke(_countResources);
        }
    }
}
