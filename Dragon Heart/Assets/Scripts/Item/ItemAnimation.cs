using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Range(0, 10)]
    [SerializeField] private float _oscillationSpeed = 1f;
    [Range(0, 1f)]
    [SerializeField] private float _oscillationHeight = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [Range(0, 360)]
    [SerializeField] private float _angleToTravel = 90f;


    private Vector3 initPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        initPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Oscillation();
        Rotate();
    }

    private void Oscillation()
    {
        float newY = Mathf.Sin(Time.time * _oscillationSpeed) * _oscillationHeight + initPos.y;

        this.transform.position = new Vector3(initPos.x, newY, initPos.z) ;
    }

    private void Rotate()
    {
        this.transform.RotateAround(this.transform.position, this.transform.up, _angleToTravel * _rotationSpeed * Time.deltaTime);
    }
}
