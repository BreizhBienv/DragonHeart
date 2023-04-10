using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOFView : MonoBehaviour
{
    [Header("Field of View Settings")]
    [SerializeField] private float _radius;
    [Range(0f, 360f)]
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;

    private bool _canSeePlayer = false;

    private GameObject _playerRef;

    [Range(0, 1)]
    [SerializeField] private float _meshResolution = 0.5f;
    [SerializeField] private float _edgeResolveIterations;
    [SerializeField] private float _edgeDistanceThreshold;
    public MeshFilter _viewMeshFilter;
    Mesh _viewMesh;

    private void Awake()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");

        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        _viewMeshFilter.mesh = _viewMesh;

        StartCoroutine(FOVRoutine());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawFieldOfView();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;

            FieldOFViewCheck();
        }
    }

    private void FieldOFViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(this.transform.position, _radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionTarget = (target.position - this.transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionTarget) < _viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(this.transform.position, target.position);

                if (!Physics.Raycast(this.transform.position, directionTarget, distanceToTarget, _obstacleMask))
                    _canSeePlayer = true;
                else
                    _canSeePlayer = false;
            }
            else
                _canSeePlayer = false;
        }
        else if (_canSeePlayer)
            _canSeePlayer = false;
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
        float stepAngleSize = _viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = transform.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCastInfo = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThreshHoldExceeded = Mathf.Abs(oldViewCast.dst - newViewCastInfo.dst) > _edgeDistanceThreshold;

                if (oldViewCast.hit != newViewCastInfo.hit || (oldViewCast.hit && newViewCastInfo.hit && edgeDstThreshHoldExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCastInfo);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCastInfo.point);
            oldViewCast = newViewCastInfo;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < _edgeResolveIterations; ++i)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThreshHoldExceeded = Mathf.Abs(minViewCast.dst - maxViewCast.dst) > _edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThreshHoldExceeded)
            {
                minAngle = angle;
                minPoint = minViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, _radius, _obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }

        return new ViewCastInfo(false, transform.position + dir * _radius, _radius, globalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool p_hit, Vector3 p_point, float p_dst, float p_angle)
        {
            hit = p_hit;
            point = p_point;
            dst = p_dst;
            angle = p_angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 p_pA, Vector3 p_pB)
        {
            pointA = p_pA;
            pointB = p_pB;
        }
    }
    public float Radius { get => _radius; }
    public float Angle { get => _viewAngle; }
    public bool CanSeePlayer { get => _canSeePlayer; }
    public GameObject PlayerRef { get => _playerRef; }
}
