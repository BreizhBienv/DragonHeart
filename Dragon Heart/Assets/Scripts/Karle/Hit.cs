using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Hit : MonoBehaviour
{

    [SerializeField] private Animator _anim;
    [SerializeField] private float _maxComboDelay = 0f;
    [SerializeField] static int _nbOfClick = 0;
    [SerializeField] private List<Collider> _hitBox;
    [SerializeField] private int _enemyLayer;
    [SerializeField] private int _rageGainUponHitting = 1;
    [SerializeField] private int _damage = 1;
    private float _nextFireTime = 0;
    private float _lastClickTime = 0;

    private Mouvements _mvmScript;
    private Sneak _snkSript;
    private Morph _morphScript;
    private PlayerManager _plMngScript;
    private FireBallShoot _frBllShtScript;

    // Start is called before the first frame update
    void Start()
    {
        _morphScript = this.GetComponent<Morph>();
        _mvmScript = this.GetComponent<Mouvements>();
        _snkSript = this.GetComponent<Sneak>();
        _plMngScript = this.GetComponent<PlayerManager>();
        _frBllShtScript = this.GetComponent<FireBallShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("DoNothing"))
        {
            UnfreezePlayer();
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            _anim.SetBool("hit1", false);
            HitboxActive(_hitBox[0], false);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            _anim.SetBool("hit2", false);
            HitboxActive(_hitBox[1], false);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            _anim.SetBool("hit3", false);
            HitboxActive(_hitBox[2], false);
            _nbOfClick = 0;
        }

        if (Time.time - _lastClickTime > _maxComboDelay)
        {
            _nbOfClick = 0;
        }
    }

    public void HitAction(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                HitAttack();
            }
        }
    }

    private void HitAttack()
    {
        if (_morphScript.IsDragon) return;

        if (Time.timeScale == 0) return;

        if (Time.time < _nextFireTime) return;

        _nbOfClick++;
        _lastClickTime = Time.time;

        if (_nbOfClick >= 3 && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            FreezePlayer();
            _anim.SetBool("hit2", false);
            _anim.SetBool("hit3", true);
            HitboxActive(_hitBox[1], false);
            HitboxActive(_hitBox[2], true);
        }

        if (_nbOfClick >= 2 && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            FreezePlayer();
            _anim.SetBool("hit1", false);
            _anim.SetBool("hit2", true);
            HitboxActive(_hitBox[0], false);
            HitboxActive(_hitBox[1], true);
        }

        Mathf.Clamp(_nbOfClick, 0, 3);

        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("DoNothing")) return;

        if (_nbOfClick >= 1 && !_snkSript.IsSneaking)
        {
            FreezePlayer();

            _anim.SetBool("hit1", true);
            HitboxActive(_hitBox[0], true);
        }
    }

    private void HitboxActive(Collider p_colliderToChangeState, bool p_state)
    {
        p_colliderToChangeState.enabled = p_state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _enemyLayer && !_morphScript.IsDragon)
        {
            _plMngScript.Rage += _rageGainUponHitting;

            var find = _frBllShtScript.EnemiesList.Find(x => x == other.gameObject);
            if (find != null) _frBllShtScript.EnemiesList.Remove(other.gameObject);

            if ((other.GetComponent<AK>().Life -= _damage) <= 0)
                _plMngScript.KillCount++;
        }
    }

    private void FreezePlayer()
    {
        _mvmScript.enabled = false;
    }

    private void UnfreezePlayer()
    {
        _mvmScript.enabled = true;

    }
}