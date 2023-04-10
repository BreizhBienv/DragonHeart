using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    private PlayerManager _plMngScript;
    private FireBallShoot _frBllShtScript;
    [SerializeField] private int _fireBallDamage = 3;
    [SerializeField] private float _fireBallSpeed = 1f;
    [SerializeField] private int _enemyLayer;
    [SerializeField] private int _obstacleLayer;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(this.transform.position + this.transform.forward * _fireBallSpeed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _obstacleLayer)
        {
            //GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
            //Destroy(gameObject, 1);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == _enemyLayer)
        {
            var find = _frBllShtScript.EnemiesList.Find(x => x == collision.gameObject);
            if (find != null) _frBllShtScript.EnemiesList.Remove(collision.gameObject);

            if ((collision.gameObject.GetComponent<AK>().Life -= _fireBallDamage) <= 0)
                _plMngScript.KillCount++; ;
            //GameObject gameObject = Instantiate(_shatterAudio, this.transform.position, Quaternion.identity);
            //GameObject gameObject2 = Instantiate(_hitAudio, this.transform.position, Quaternion.identity);

            //Destroy(gameObject, 1);
            //Destroy(gameObject2, 1);

            Destroy(this.gameObject);
        }
    }

    public PlayerManager PlayerManagerScript { set => _plMngScript = value; }
    public FireBallShoot FireBallShootScript { set => _frBllShtScript = value; }
}
