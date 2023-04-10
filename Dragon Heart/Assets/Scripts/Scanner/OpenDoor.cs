using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{

    private bool _activated = false;
    [SerializeField] private Animator Anim_porte;
    [SerializeField] private bool _finalDoor = false;
    public AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SesameOuvreToi(PlayerManager p_playerManager)
    {
        if (_activated) return;

        if (_finalDoor)
            SceneManager.LoadScene(1);

        p_playerManager.KeyCounter--;
        Anim_porte.SetBool("open",true);
        audiosource.Play();
    }
}
