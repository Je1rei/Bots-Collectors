using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _holdDistance;
    [SerializeField] private float _throwForce;

    private PickingObject _pickedObject;

    public bool CanPick => _pickedObject == null;
    public float HoldDistance => _holdDistance;

    public event Action PickedUp;

    public Apple Apple { get; private set; }

    private void OnTriggerEnter(Collider unit)
    {
        if (unit != null)
        {
            if (unit.TryGetComponent(out PickingObject pickingObject) && pickingObject.gameObject == Apple.gameObject)
            {
                PickUp(pickingObject);
            }
        }
    }

    public void SetApple(Apple apple) => Apple = apple;

    public void PickUp(PickingObject pickingObject)
    {
        if (CanPick == true)
        {
            _pickedObject = pickingObject;
            _pickedObject.PickUp(transform, _holdDistance);
            PickedUp?.Invoke();
        }
    }

    public PickingObject Drop()
    {
        ThrowObject(Vector3.zero);
        PickingObject picked = _pickedObject;

        _pickedObject = null;

        return picked;
    }

    private void ThrowObject(Vector3 force)
    {
        _pickedObject.Throw(force);
    }
}
