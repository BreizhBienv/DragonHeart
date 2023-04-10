using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK : MonoBehaviour
{
    [SerializeField] private int _life = 3;
    //[SerializeField] private int _hitBoxLayer;

    [Header("AI Settings")]
    [SerializeField] private LayerMask _playerM;
    private GameObject _player;

    [Header("Attacking")]
    [SerializeField] private float _timeBtwAttack = 0.5f;
    [SerializeField] private float _timeOfAttack = 2.5f;
    [SerializeField] private float _reloadTime = 3f;
    [SerializeField] private GameObject _bulletPrefab;
    private bool _isAttacking = false;
    private bool _isReloading = false;
    public AudioSource audiosource;

    FieldOFView _fieldOfView;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _fieldOfView = this.GetComponent<FieldOFView>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_life <= 0)
            Destroy(this.gameObject);

        if (_fieldOfView.CanSeePlayer) Attack();
    }


    private void Attack()
    {
        this.transform.LookAt(_player.transform);

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        if (_isAttacking || _isReloading)
            yield break;
         audiosource.Play();
        _isAttacking = true;

        float coroutineStartTime = Time.time;

        while ((Time.time - coroutineStartTime) < _timeOfAttack && _fieldOfView.CanSeePlayer)
        {
            ShootPlayer();
            yield return new WaitForSeconds(_timeBtwAttack);
        }

        _isAttacking = false;
        _isReloading = true;

        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(_reloadTime);

        _isReloading = false;
    }

    private void ShootPlayer()
    {
        GameObject bullet = Instantiate(_bulletPrefab, this.transform.position + this.transform.forward, this.transform.rotation);

        bullet.transform.position += bullet.transform.forward;
    }



    public int Life { get => _life; set => _life = value; }
}
