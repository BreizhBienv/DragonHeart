using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);

    public static bool HasConstraint(this RigidbodyConstraints p_rbConstraint, RigidbodyConstraints p_constraint)
    {
        return (p_rbConstraint & p_constraint) != RigidbodyConstraints.None;
    }
}
