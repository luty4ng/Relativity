using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class SimulatedBody : GravityObject
{
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    public bool enableRelativity = false;
    Transform meshHolder;

    public Vector3 velocity { get; private set; }
    public float mass { get; private set; }
    Rigidbody rb;
    Observer theOnlyObserver;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        velocity = initialVelocity;
        if (enableRelativity)
            theOnlyObserver = FindObjectOfType<Observer>();
    }

    public virtual void OnUpdateTimeStepMultipiler()
    {
        if (enableRelativity)
        {
            if (theOnlyObserver == null)
            {
                Debug.LogError("The only observer is not exist.");
                return;
            }
            
            float sqrDst = (theOnlyObserver.rb.position - this.rb.position).sqrMagnitude;
            float schwarzschildRadius = 2 * mass * Universe.current.GravitationalConstant;
            timeStepMultipier = Mathf.Sqrt(1 - (schwarzschildRadius / sqrDst));
        }
    }

    public void UpdateVelocity(SimulatedBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;
                Vector3 acceleration = forceDir * Universe.current.GravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        velocity += acceleration * timeStep * timeStepMultipier;
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + velocity * timeStep * timeStepMultipier);
    }

    void OnValidate()
    {
        mass = surfaceGravity * radius * radius / Universe.DefaultGravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return rb;
        }
    }

    public Vector3 Position
    {
        get
        {
            return rb.position;
        }
    }
}