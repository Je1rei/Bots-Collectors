using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(MeshRenderer))]
public abstract class Resource<T> : MonoBehaviour where T : MonoBehaviour 
{
    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
    }

    public void SetIsBusy() => IsBusy = true;

    public void SetIsFree() => IsBusy = false;
}
