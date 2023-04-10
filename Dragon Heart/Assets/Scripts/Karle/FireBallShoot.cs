using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FireBallShoot : MonoBehaviour
{
    [SerializeField] private GameObject _fireBallPrefab;
    [SerializeField] private GameObject _fireBallSpawn;
    [SerializeField] private float _fireBallCooldown = 0.5f;
    [SerializeField] private int _enemyLayer;
    public AudioSource audiosource;

    private Morph _mphScript;
    private PlayerManager _plMngScript;

    private List<GameObject> _enemies = new List<GameObject>(0);
    private bool _readyToShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        _mphScript = this.GetComponent<Morph>();
        _plMngScript = this.GetComponent<PlayerManager>();
       
    }


    public void ShootFireBall(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed && Time.timeScale != 0)
            {
                audiosource.Play();
                ThrowFireBall();
            }
        }
    }

    private void ThrowFireBall()
    {
        if (!_mphScript.IsDragon || !_readyToShoot) return;

        GameObject enemyToTarget = SearchForRandomEnemy();

        if (enemyToTarget == null) return;

        GameObject fireBall = Instantiate(_fireBallPrefab, _fireBallSpawn.transform.position + this.transform.forward, this.transform.rotation);

        FireBallBehaviour fireBallBehaviour = fireBall.GetComponent<FireBallBehaviour>();

        fireBallBehaviour.FireBallShootScript = this;
        fireBallBehaviour.PlayerManagerScript = _plMngScript;

        fireBall.transform.position += fireBall.transform.forward;
        fireBall.transform.LookAt(enemyToTarget.transform);

        StartCoroutine(ShootCooldown());
    }

    private GameObject SearchForRandomEnemy()
    {
        if (_enemies.Count == 1) return _enemies[0];

        if (_enemies.Count == 0) return null;

        return _enemies[Random.Range(0, _enemies.Count - 1)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _enemyLayer && _mphScript.IsDragon)
        {
            _enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _enemyLayer && _mphScript.IsDragon)
        {
            _enemies.Remove(other.gameObject);
        }
    }

    private IEnumerator ShootCooldown()
    {
        _readyToShoot = false;

        yield return new WaitForSeconds(_fireBallCooldown);

        _readyToShoot = true;
    }

    public List<GameObject> EnemiesList { get => _enemies; set => _enemies = value; }
}
