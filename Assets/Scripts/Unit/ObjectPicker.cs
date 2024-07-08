using UnityEngine;
using System;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _holdDistance;
    [SerializeField] private float _throwForce;

    private PickingObject _pickedObject;

    public bool CanPick => _pickedObject == null;
    public float HoldDistance => _holdDistance;

    public event Action PickedUp;

    private void OnTriggerEnter(Collider unit)
    {
        if (unit.TryGetComponent(out PickingObject pickingObject))
        {
            PickUp(pickingObject);
        }
    }

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
