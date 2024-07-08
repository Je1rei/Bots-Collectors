using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void FixedUpdate()
    {
        transform.rotation = _target.rotation;
    }
}
