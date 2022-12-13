using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickKit;

public class Universe : MonoSingletonBase<Universe> {
    public const float DefaultGravitationalConstant = 0.0001f;
    public const float DefaultPhysicsTimeStep = 0.01f;
    public float GravitationalConstant = DefaultGravitationalConstant;
    public float PhysicsTimeStep = DefaultPhysicsTimeStep;
    
}