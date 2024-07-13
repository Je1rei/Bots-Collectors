using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _delayScan = 3;

    private Coroutine _currentCoroutine;

    public event Action<Apple> Scanned;

    private void Awake()
    {
        SetupScan();
    }

    private void SetupScan()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        else
            _currentCoroutine = StartCoroutine(Scan());
    }

    private IEnumerator Scan()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_delayScan);

        while (enabled)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);

            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.TryGetComponent(out Apple resource))
                {
                    Scanned?.Invoke(resource);
                }
            }

            yield return waitTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
