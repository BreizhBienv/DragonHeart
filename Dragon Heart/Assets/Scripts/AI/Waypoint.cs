using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Color _gizmoColor;
    [SerializeField] private float _radius = 1f;

    private void Start()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }
}
