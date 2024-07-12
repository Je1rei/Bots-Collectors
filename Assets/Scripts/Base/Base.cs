using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private FactoryBase _spawnerBase;
    [SerializeField] private SpawnerApple _spawnerApple;
    [SerializeField] private FactoryUnit _spawnerUnits;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private AppleStorage _storage;
    [SerializeField] private FlagHandler _flagHandler;
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private Flag _flag;

    private Queue<Apple> _freeScannedResources = new Queue<Apple>();

    public event Action Selected;

    public bool HasFlag { get; private set; }

    private void OnMouseDown()
    {
        Selected?.Invoke();
    }

    private void OnEnable()
    {
        _flagHandler.Installed += GiveFlagDelivery;
        _scanner.Scanned += AddScanned;

        foreach (var unit in _units)
        {
            unit.ReachedStorage += GetDelivery;
            unit.CreatedBase += InitializeBase;
        }
    }

    private void OnDisable()
    {
        _flagHandler.Installed -= GiveFlagDelivery;
        _scanner.Scanned -= AddScanned;

        foreach (var unit in _units)
        {
            unit.ReachedStorage -= GetDelivery;
            unit.CreatedBase -= InitializeBase;
        }
    }

    private void Awake()
    {
        _flag = null;
        HasFlag = false;
    }

    private void ProcessGiveOrders(int countValue)
    {
        int minCountUnitsCreateNewBase = 1;
        int priceCreateBase = 5;
        int priceCreateUnit = 3;

        Unit newUnit = null;

        foreach (Unit unit in _units)
        {
            if (unit.IsFree)
            {
                if (HasFlag && _units.Count > minCountUnitsCreateNewBase)
                {
                    if (_storage.CountResources >= priceCreateBase)
                    {
                        _storage.DecreaseResource(priceCreateBase);
                        unit.MoveToTarget(_flag);
                        _flag = null;
                        HasFlag = false;

                        return;
                    }
                }
                else if (_storage.CountResources >= priceCreateUnit)
                {
                    _storage.DecreaseResource(priceCreateUnit);
                    newUnit = _spawnerUnits.Create();
                }

                Apple target = TryGiveOrderDelivery();

                if (target != null)
                {
                    unit.MoveToTarget(target);
                }
            }
        }

        if (newUnit != null)
            AddUnit(newUnit);
    }

    private void GiveFlagDelivery(Flag flag)
    {
        _flag = flag;
        HasFlag = true;
    }

    private Apple TryGiveOrderDelivery()
    {
        foreach (Apple scanned in _freeScannedResources)
        {
            if (scanned.IsBusy == false)
            {
                _freeScannedResources.Dequeue();
                scanned.SetIsBusy();

                return scanned;
            }
        }

        return null;
    }

    private void GetDelivery(Apple apple)
    {
        if (apple.IsBusy == true)
        {
            _spawnerApple.ReleasePool(apple);
            apple.SetIsFree();
            _storage.IncreaseResource();
        }
    }

    private void AddUnit(Unit unit)
    {
        unit.SetStorage(_storage.transform);
        unit.SetBase(this);
        unit.SetFactory(_spawnerBase);
        unit.ReachedStorage += GetDelivery;
        unit.CreatedBase += InitializeBase;

        _units.Add(unit);
    }

    private void InitializeBase(Base newbase, Unit unit)
    {
        newbase.SetSpawnerApple(_spawnerApple);
        newbase.SetSpawnerBase(_spawnerBase);
        newbase.AddUnit(unit);
        TransferUnit(unit, newbase);
        _units.Remove(unit);
    }

    private void TransferUnit(Unit unit, Base newbase)
    {
        unit.ReachedStorage -= GetDelivery;
        unit.CreatedBase -= InitializeBase;

        unit.SetBase(newbase);
        unit.ReachedStorage += newbase.GetDelivery;
        unit.CreatedBase += newbase.InitializeBase;
    }

    private void SetSpawnerApple(SpawnerApple spawner) => _spawnerApple = spawner;

    private void SetSpawnerBase(FactoryBase spawner) => _spawnerBase = spawner;

    private void AddScanned(Apple scanned)
    {
        if (_freeScannedResources.Contains(scanned) == false)
        {
            _freeScannedResources.Enqueue(scanned);
        }

        ProcessGiveOrders(_storage.CountResources);
    }
}