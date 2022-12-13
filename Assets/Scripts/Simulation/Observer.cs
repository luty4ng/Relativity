using UnityEngine;

public class Observer : MonoBehaviour
{
    public Camera cam;
    public Rigidbody rb;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
    }
}