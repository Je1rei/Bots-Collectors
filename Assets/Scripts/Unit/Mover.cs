using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private float _speed;
    private float _overlapDistance = 0.5f;

    private Transform _currentTarget;
    private Coroutine _currentCoroutine;

    public Transform SetCurrentTarget(Transform target) => _currentTarget = target;
    public bool IsWalk { get; private set; }

    private void Awake()
    {
        IsWalk = false;
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;

        if (_currentTarget != null)
        {
            _unit.SetIsBusy();
            SetupMove();
        }
    }

    public void SetupMove()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (_currentTarget != null)
        {
            if (_unit.Picker.CanPick == false && _currentTarget != _unit.Storage)
            {
                _currentTarget = _unit.Storage;
            }

            MoveTo(_currentTarget);

            yield return null;
        }
    }

    private void MoveTo(Transform target)
    {
        var step = _speed * Time.deltaTime;
        _unit.TurnAnimator();

        if (IsTargetReached(target) == false)
        {
            IsWalk = true;
            Vector3 direction = (target.position - transform.position).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, step);
            transform.LookAt(target.position);

            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else if (_unit.Flag != null && target == _unit.Flag.transform)
        {
            IsWalk = false;
            _unit.CreateBase();
            _unit.SetIsFree();
        }
        else if (target == _unit.Storage)
        {
            IsWalk = false;
            _unit.ContactStorage();
            _currentTarget = null;
            _unit.SetIsFree();
        }
    }

    private bool IsTargetReached(Transform target)
    {
        return transform.position.IsEnoughClose(new Vector3(target.position.x, target.position.y, target.position.z), _overlapDistance);
    }
}
