using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] private GameObject _ventToDisable;
    [SerializeField] private int _PlayerLayer;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _PlayerLayer) return;

        Sneak sneak = other.gameObject.GetComponent<Sneak>();
        Morph morph = other.gameObject.GetComponent<Morph>();

        if (!sneak.IsSneaking || morph.IsDragon) return;

        _ventToDisable.SetActive(true);
    }
}
