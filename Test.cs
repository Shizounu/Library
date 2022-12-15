using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shizounu.Library.ScriptableArchitecture;


public class Test : MonoBehaviour
{
    public BoolReference boolRef;
    public FloatReference floatRef;
    public Vector3Reference Vec3Ref;
    public IntReference IntRef;

    [Space()]

    public VariableReference<Vector2> vec2Ref;
}
