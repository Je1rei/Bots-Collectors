using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private SpawnerApple _spawner;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private AppleStorage _storage;
    [SerializeField] private List<Unit> _units;

    private Queue<Apple> _freeScannedResources = new Queue<Apple>();
    private HashSet<Apple> _busyScannedResources = new HashSet<Apple>();

    public event Action<Transform> GivedOrder;

    private void OnEnable()
    {
        _scanner.Scanned += AddScanned;

        foreach (var unit in _units)
        {
            unit.ReachedStorage += GetDelivery;
        }
    }

    private void OnDisable()
    {
        _scanner.Scanned -= AddScanned;

        foreach (var unit in _units)
        {
            unit.ReachedStorage -= GetDelivery;
        }
    }

    private void Update()
    {
        if (_freeScannedResources.Count > 0)
        {
            foreach (Unit unit in _units)
            {
                if (unit.IsFree)
                    unit.SetTarget(TryGiveOrder());
            }
        }
    }

    public Transform TryGiveOrder()
    {
        if (_freeScannedResources.TryDequeue(out Apple target))
        {
            if (_busyScannedResources.Add(target))
            {
                return target.transform;
            }
        }

        return null;
    }

    private void GetDelivery(Apple apple)
    {
        if (_busyScannedResources.Contains(apple))
        {
            _busyScannedResources.Remove(apple);
            _spawner.ReleasePool(apple);

            _storage.IncreaseResource();
        }
    }

    private void AddScanned(Apple scanned)
    {
        if (_freeScannedResources.Contains(scanned) == false && _busyScannedResources.Contains(scanned) == false)
        {
            _freeScannedResources.Enqueue(scanned);
        }
    }
}