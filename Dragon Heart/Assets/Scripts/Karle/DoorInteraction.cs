using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private int _doorLayer;
    PlayerManager _playerManager;
    private GameObject _other;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = this.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _doorLayer)
        {
            _other = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _doorLayer)
        {
            _other = null;
        }
    }

    public void OpenDoor(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                Opened();
            }
        }
    }


    private void Opened()
    {
        if (_other == null || _playerManager.KeyCounter <= 0) return;

        _other.GetComponent<OpenDoor>().SesameOuvreToi(_playerManager);
    }
}
