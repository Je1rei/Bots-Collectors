using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(MeshRenderer))]
public abstract class Resource<T> : MonoBehaviour where T : MonoBehaviour { }
