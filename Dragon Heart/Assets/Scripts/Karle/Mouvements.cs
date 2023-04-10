using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouvements : MonoBehaviour
{
    [Header("Mouvement's settings")]
    [SerializeField] private float _baseSpeed = 5;
    [SerializeField] private GameObject _mainCamera;
    private float _currentSpeed;
    private Rigidbody _rb;
    private Vector3 _input;

    [SerializeField] private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _currentSpeed = _baseSpeed;
    }

    private void Update()
    {
        Look();
        DoNothingAnimation();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movements();
    }

    private void Movements()
    {
        _rb.MovePosition(this.transform.position + (this.transform.forward * _input.magnitude) * _currentSpeed * Time.fixedDeltaTime);

    }

    private void DoNothingAnimation()
    {
        if (_input == Vector3.zero)
        {
            _anim.SetBool("running", false);
            return;
        }


        _anim.SetBool("running", true);
    }

    private void Look()
    {
        if (_input != Vector3.zero)
        {
            //var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);

            var rot = _mainCamera.transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0, rot.y, 0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            Vector3 result = isoMatrix.MultiplyPoint3x4(_input);

            this.transform.rotation = Quaternion.LookRotation(result, Vector3.up);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            _input = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }
    }

    public float CurrentSpeed { set => _currentSpeed = value; }
    public float BaseSpeed { get => _baseSpeed; }

    public Vector3 Input { get => _input; }
}
