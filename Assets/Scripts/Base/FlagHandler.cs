using System;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _prefab;
    [SerializeField] private Base _base;
    [SerializeField] private LayerMask _groundLayer;

    private Flag _flag;
    private bool _isMoving = false;

    public event Action<Flag> Installed;

    private void OnEnable()
    {
        _base.Selected += SetupMove;
    }

    private void OnDisable()
    {
        _base.Selected -= SetupMove;
    }

    private void Awake()
    {
        _isMoving = false;
    }

    private void Update()
    {
        if (_isMoving)
        {
            Move();
        }
    }

    private void SetupMove()
    {
        if (_flag == null)
        {
            _flag = Instantiate(_prefab);
        }

        _isMoving = true;
    }

    private void Move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _flag.transform.position = hit.point;

            if (Input.GetMouseButtonUp(0))
            {
                _flag.transform.position = hit.point;
                _isMoving = false;
                Installed?.Invoke(_flag);
            }
        }
    }
}