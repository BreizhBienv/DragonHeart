using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<GameObject> _patrolPoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _secondToWaitOnWayPoint = 1f;
    [SerializeField] private float _distanceToChangePatrolPoint = 1f;

    private int _currPatrolPoint = 0;
    private bool _waiting = false;
    private GameObject _destination;
    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();

        _destination = _patrolPoints[0];
    }

    private void FixedUpdate()
    {
        if (!_waiting)
            GoToDestination();

        if (_waiting)
            RotateToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        SetNextDestination();
    }

    private float CheckDistanceToPatrolPoint()
    {
        return Vector3.Distance(_destination.transform.position, this.transform.position);
    }

    private void SetNextDestination()
    {
        if (CheckDistanceToPatrolPoint() > _distanceToChangePatrolPoint) return;

        StartCoroutine(WaitOnWayPoint());

        _currPatrolPoint = (_currPatrolPoint + 1) % _patrolPoints.Count;
        _destination = _patrolPoints[_currPatrolPoint];
    }

    private void GoToDestination()
    {
        this.transform.LookAt(_destination.transform.position);
        _rb.MovePosition(this.transform.position + this.transform.forward * _speed * Time.fixedDeltaTime);
    }

    private IEnumerator WaitOnWayPoint()
    {
        _waiting = true;

        yield return new WaitForSeconds(_secondToWaitOnWayPoint);

        _waiting = false;
    }

    private void RotateToTarget()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = _destination.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = _speedRotation * Time.fixedDeltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
