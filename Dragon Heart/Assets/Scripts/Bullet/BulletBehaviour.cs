using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private GameObject _playerRef;
    [SerializeField] private int _playerLayer;
    [SerializeField] private int _obstacleLayer;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private Rigidbody _rb;
    PlayerManager _playerManager;

    //[SerializeField] private GameObject _particleDeath;

    //[SerializeField] private GameObject _hitAudio;
    //[SerializeField] private GameObject _shatterAudio;

    // Start is called before the first frame update
    void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _rb = this.GetComponent<Rigidbody>();
        _playerManager = _playerRef.GetComponent<PlayerManager>();
    }


    void FixedUpdate()
    {
        _rb.MovePosition(this.transform.position + this.transform.forward * _speed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        //GameObject Instant = Instantiate(_particleDeath, this.transform.position, this.transform.rotation);
        //Destroy(Instant, 0.5f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _obstacleLayer)
        {
            //GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
            //Destroy(gameObject, 1);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == _playerLayer)
        {
            _playerManager.CurrLife -= _damage;
            //GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
            //GameObject gameObject2 = Instantiate(_hitAudio, this.transform.position, Quaternion.identity);

            //Destroy(gameObject, 1);
            //Destroy(gameObject2, 1);

            Destroy(this.gameObject);
        }
    }
}
