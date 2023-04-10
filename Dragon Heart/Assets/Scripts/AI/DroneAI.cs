using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    private Patrol _patrolScript;
    private FieldOFView _fieldOFViewScript;
    private GameObject _player;
    private PlayerManager _playerManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _patrolScript = this.GetComponent<Patrol>();
        _fieldOFViewScript = this.GetComponent<FieldOFView>();
        _playerManagerScript = _player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GamineSpoted();
    }

    private void GamineSpoted()
    {
        if (_fieldOFViewScript.CanSeePlayer)
        {
            this.transform.LookAt(_player.transform);
            _patrolScript.enabled = false;
            _playerManagerScript.HasDied = true;
        }
    }
}
