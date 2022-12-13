using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    SimulatedBody[] bodies;
    static NBodySimulation instance;

    void Awake()
    {
        bodies = FindObjectsOfType<SimulatedBody>();
        Time.fixedDeltaTime = Universe.current.PhysicsTimeStep;
        Debug.Log("Setting fixedDeltaTime to: " + Universe.current.PhysicsTimeStep);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity(acceleration, Universe.current.PhysicsTimeStep);
            //bodies[i].UpdateVelocity (bodies, Universe.current.PhysicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(Universe.current.PhysicsTimeStep);
        }
    }

    public static Vector3 CalculateAcceleration(Vector3 point, SimulatedBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * Universe.current.GravitationalConstant * body.mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static SimulatedBody[] Bodies
    {
        get
        {
            return Instance.bodies;
        }
    }

    static NBodySimulation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NBodySimulation>();
            }
            return instance;
        }
    }
}