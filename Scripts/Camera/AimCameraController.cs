using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimCameraController : MonoBehaviour
{
    public NetworkCharacterController _controller;

    [SerializeField] private Transform _cameraBase, _cameraFollow;
    [SerializeField] private float _xSens, _ySens;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private bool _invertY;
    [SerializeField] private float _maxYAngle;

    private float _xRot, _yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        _cameraBase.transform.position = _controller.transform.position;

         float _xInput = Input.GetAxis("Mouse X");
         float _yInput = Input.GetAxis("Mouse Y");

        _xRot += _xInput * Time.deltaTime * _xSens;
        _yRot += _yInput * Time.deltaTime * _ySens * (_invertY ? -1 : 1);
        _yRot = Mathf.Clamp(_yRot, -_maxYAngle, _maxYAngle);

        _cameraBase.rotation = Quaternion.Euler(0,_xRot , 0);
        _cameraFollow.localRotation = Quaternion.Euler(_yRot,0,0);
    }

    public CinemachineVirtualCamera GetVirtualCamera()
    {
        return _virtualCamera;
    }

    public void ResetDirection()
    {
        _xRot = Camera.main.transform.eulerAngles.y;
        float xAngle = Camera.main.transform.localEulerAngles.x;
        xAngle = xAngle > 180 ? -180 + (xAngle%180) : xAngle;
        _yRot = Mathf.Clamp(xAngle, -_maxYAngle, _maxYAngle);
    }
}
