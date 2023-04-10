using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private int _boxLayer;

    [Header("Push Settings")]
    [SerializeField] private float _forceMagnitude = 1.0f;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float _reducedSpeed = 0.5f;
    [Range(10f, 60f)]
    [SerializeField] private float _angleToLock = 30f;

    private bool _isPushing = false;

    #region Reset Variable
    private Vector3 _initPos = Vector3.zero;
    private Quaternion _initRot = Quaternion.identity;
    #endregion

    #region Script & Components
    private Morph _morph;
    private Mouvements _mouvement;
    private Rigidbody _rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _initPos = this.transform.position;
        _initRot = this.transform.rotation;

        _morph = this.GetComponent<Morph>();
        _mouvement = this.GetComponent<Mouvements>();
        _rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BoxInteract(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed && Time.timeScale != 0)
            {
                _isPushing = true;
            }

            if (context.canceled)
            {
                _isPushing = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != _boxLayer) return;

        Push(collision.gameObject.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != _boxLayer) return;

        StopPushing(collision.gameObject.GetComponent<Rigidbody>());
    }

    private void Push(Rigidbody p_boxRigidBody)
    {
        if (_morph.IsDragon || p_boxRigidBody == null || !_isPushing) return;

        p_boxRigidBody.isKinematic = false;
        Vector3 forceDirection = p_boxRigidBody.transform.position - this.transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();

        LockAxis(p_boxRigidBody);
        _mouvement.CurrentSpeed = _mouvement.BaseSpeed * _reducedSpeed;

        p_boxRigidBody.AddForceAtPosition(forceDirection * _forceMagnitude, this.transform.position, ForceMode.Impulse);
    }

    private void StopPushing(Rigidbody p_boxRigidBody)
    {
        _mouvement.CurrentSpeed = _mouvement.BaseSpeed;

        FreeAxis(p_boxRigidBody);

        if (p_boxRigidBody == null) return;

        p_boxRigidBody.isKinematic = true;

    }

    private void LockAxis(Rigidbody p_boxRigidBody)
    {
        var AngleToXAxis = Vector3.Angle(this.transform.forward, Vector3.right);
        var AngleToZAxis = Vector3.Angle(this.transform.forward, Vector3.forward);

        if (AngleToXAxis >= (90 - _angleToLock) && AngleToXAxis <= (90 + _angleToLock))
        {
            p_boxRigidBody.constraints |= RigidbodyConstraints.FreezePositionX;
        }
        else if (AngleToZAxis >= 90 - _angleToLock && AngleToZAxis <= 90 + _angleToLock)
        {
            p_boxRigidBody.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void FreeAxis(Rigidbody p_boxRigidBody)
    {
        if (p_boxRigidBody.constraints.HasConstraint(RigidbodyConstraints.FreezePositionX))
        {
            p_boxRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionX;
        }
        else if (p_boxRigidBody.constraints.HasConstraint(RigidbodyConstraints.FreezePositionZ))
        {
            p_boxRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }
}
