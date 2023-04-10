using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Sneak : MonoBehaviour
{
    [Header("Sneaking Settings")]
    [Range(0.1f, 0.9f)]
    [SerializeField] private float _reducedSpeed = 0.5f;
    private Mouvements _mouvementScript;
    private Morph _morphScript;
    private bool _isSneaking = false;



    [SerializeField] private Animator _anim;

    #region capsule collider
    private CapsuleCollider _capsuleCollider;
    private float _initHeight;
    [Range(0, 1)]
    [SerializeField] private float _sneakHeight;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mouvementScript = this.GetComponent<Mouvements>();
        _morphScript = this.GetComponent<Morph>();
        _capsuleCollider = this.GetComponent<CapsuleCollider>();
        if (_capsuleCollider != null)
            _initHeight = _capsuleCollider.height;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Sneaking(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed && Time.timeScale != 0)
            {
                CurrentlySneaking();
            }
        }

        if (context.canceled)
        {
            StandUp();
        }
    }

    private void CurrentlySneaking()
    {
        if (!_morphScript.IsDragon && _anim.GetCurrentAnimatorStateInfo(0).IsName("DoNothing"))
        {
            _mouvementScript.CurrentSpeed = _mouvementScript.BaseSpeed * _reducedSpeed;

            _capsuleCollider.height = _sneakHeight;

            _isSneaking = true;

            _anim.SetBool("sneaking", true);
        }
    }

    private void StandUp()
    {
        _capsuleCollider.height = _initHeight;

        _isSneaking = false;
        _anim.SetBool("sneaking", false);

        _mouvementScript.CurrentSpeed = _mouvementScript.BaseSpeed;
    }

    public bool IsSneaking { get => _isSneaking; }
}
