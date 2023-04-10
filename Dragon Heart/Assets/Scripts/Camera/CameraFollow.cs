using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _player;
    [Range(0.01f, 1.0f)]
    [SerializeField] private float _smoothFactor = 0.5f;

    private Vector3 _cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = this.transform.position - _player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = _player.position + _cameraOffset;

        this.transform.position = Vector3.Slerp(this.transform.position, newPos, _smoothFactor);
    }
}
