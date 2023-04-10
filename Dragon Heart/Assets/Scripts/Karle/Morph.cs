using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Morph : MonoBehaviour
{
    [Header("Morph Settings")]
    [SerializeField] private float _morphTime = 10.0f; //in sec
    [SerializeField] private float _morphCooldown = 10.0f; //in sec
    [SerializeField] private GameObject _dragonForm;
    [SerializeField] private GameObject _humanForm;
    public AudioSource audiosource;


    private bool _isDragon = false;
    private bool _inCooldown = false;
    private PlayerManager _plMngScript;

    // Start is called before the first frame update
    void Start()
    {
        _plMngScript = this.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MorphToDragon(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed && Time.timeScale != 0)
            {
                if (_plMngScript.Rage >= _plMngScript.RageNeeded)
                    StartCoroutine(Transform());
            }
        }
    }

    IEnumerator Transform()
    {
        if (_isDragon || _inCooldown)
            yield break;

        audiosource.Play();
        _isDragon = true;


        _plMngScript.Rage = 0;

        _dragonForm.SetActive(true);
        _humanForm.SetActive(false);

        yield return new WaitForSeconds(_morphTime);

        _isDragon = false;
        _inCooldown = true;

        _dragonForm.SetActive(false);
        _humanForm.SetActive(true);

        yield return new WaitForSeconds(_morphCooldown);

        _inCooldown = false;
    }

    public bool IsDragon { get => _isDragon; }
}
