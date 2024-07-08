using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Transform _storage;
    [SerializeField] private ObjectPicker _picker;
    [SerializeField] private UnitMovement _movement;

    public bool IsFree { get; private set; }
    public Transform Storage => _storage;
    public ObjectPicker Picker => _picker;

    public event Action<Apple> ReachedStorage;

    private void OnEnable()
    {
        _picker.PickedUp += PickedUp;
    }

    private void OnDisable()
    {
        _picker.PickedUp -= PickedUp;
    }

    private void Awake()
    {
        IsFree = true;
    }

    public void SetIsFree() => IsFree = true;
    public void SetIsBusy() => IsFree = false;

    public void ContactStorage()
    {
        ReachedStorage?.Invoke(DropPicked());
    }
    
    public void SetTarget(Transform transform)
    {
        _movement.SetTarget(transform);
    }

    public Apple DropPicked()
    {
        PickingObject pickedObject = _picker.Drop();
        Apple apple = pickedObject.GetComponent<Apple>();

        return apple;
    }

    private void PickedUp()
    {
        _movement.SetCurrentTarget(_storage);
        _movement.SetupMove();
    }
}
