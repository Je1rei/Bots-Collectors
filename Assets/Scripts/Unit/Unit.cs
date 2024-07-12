using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private FactoryBase _factory;
    [SerializeField] private Transform _storage;
    [SerializeField] private ObjectPicker _picker;
    [SerializeField] private Mover _mover;

    private int _isWalkHash = Animator.StringToHash("isWalk");
    private Animator _animator;

    public event Action<Apple> ReachedStorage;
    public event Action<Base, Unit> CreatedBase;

    public Flag Flag { get; private set; }
    public bool IsFree { get; private set; }
    public bool IsCreatingBase => Flag != null;

    public Transform Storage => _storage;
    public ObjectPicker Picker => _picker;

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
        _animator = GetComponent<Animator>();
        IsFree = true;
    }

    public void SetIsFree() => IsFree = true;

    public void SetIsBusy() => IsFree = false;

    public void SetStorage(Transform transform) => _storage = transform;

    public void SetBase(Base newBase) => _base = newBase;

    public void SetFactory(FactoryBase factory) => _factory = factory;

    public void ContactStorage()
    {
        ReachedStorage?.Invoke(DropPicked());
    }

    public void MoveToTarget(Apple apple)
    {
        _picker.SetApple(apple);
        _mover.SetTarget(apple.transform);
    }

    public void MoveToTarget(Flag flag)
    {
        Flag = flag;
        _mover.SetTarget(flag.transform);
    }

    public void CreateBase()
    {
        CreatedBase?.Invoke(_factory.Create(Flag.transform), this);
        Destroy(Flag.gameObject);

        Flag = null;
    }

    public Apple DropPicked()
    {
        PickingObject pickedObject = _picker.Drop();
        Apple apple = pickedObject.GetComponent<Apple>();

        return apple;
    }

    public void TurnAnimator()
    {
        _animator.SetBool(_isWalkHash, _mover.IsWalk);
    }

    private void PickedUp()
    {
        _mover.SetCurrentTarget(_storage);
        _mover.SetupMove();
    }
}
