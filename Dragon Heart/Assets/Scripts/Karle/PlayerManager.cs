using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int _keyLayer;
    [SerializeField] private int _healthPackLayer;

    [Header("Player Settings")]
    [SerializeField] private int _baseLife = 3;
    [SerializeField] private int _rageNeededToMorph = 10;
    private int _currRage = 0;
    private int _killCount = 0;
    public AudioSource audiosource;


    private Morph _mrphScript;

    private bool _hasDied;
    private int _currentLife;
    private int _keyCounter = 0;

    [Header("UI")]
    [SerializeField] private List<Image> _hearts;
    [SerializeField] private Image _key;
    [SerializeField] private Text _rageUI;
    [SerializeField] private Image _controls;
    private bool _StartedPlaying = false;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = _baseLife;
        _mrphScript = this.GetComponent<Morph>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HealthUI());
        StartCoroutine(keyUI());
        StartCoroutine(RageDisplay());

        if (_currentLife <= 0)
            _hasDied = true;

        if (_hasDied)
            ReloadScene();
    }

    private IEnumerator HealthUI()
    {
        for (int i = 0; i < _hearts.Count; ++i)
        {
            if (i < _currentLife) {

                _hearts[i].enabled = true;
               
            }
                
            else
                _hearts[i].enabled = false;
        }


        yield return null;
    }

    private IEnumerator RageDisplay()
    {
        _rageUI.text = _currRage + "/" + _rageNeededToMorph;

        yield return null;
    }

    private IEnumerator keyUI()
    {
        if (_keyCounter <= 0)
            _key.enabled = false;
        else
            _key.enabled = true;

        yield return null;
    }

    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_mrphScript.IsDragon) return;

        if (other.gameObject.layer == _keyLayer)
        {
            _keyCounter++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == _healthPackLayer)
        {
            if (_currentLife < _baseLife) 
                _currentLife++; audiosource.Play();
            

            Destroy(other.gameObject);
        }
    }

    public void Play(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed && !_StartedPlaying)
            {
                _controls.enabled = false;
                Time.timeScale = 1;
                _StartedPlaying = true;
            }
        }
    }

    public int CurrLife { get => _currentLife; set => _currentLife = value; }
    public int KeyCounter { get => _keyCounter; set => _keyCounter = value; }
    public bool HasDied { get => _hasDied; set => _hasDied = value; }
    public int Rage { get => _currRage; set => _currRage = value; }
    public int RageNeeded { get => _rageNeededToMorph; set => _rageNeededToMorph = value; }
    public int KillCount { get => _killCount; set => _killCount = value; }
}
