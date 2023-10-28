using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkCharacterController : NetworkBehaviour
{

    public event Action OnMoveBegin;
    public event Action OnMoveEnd;
    public event Action OnSprintBegin;
    public event Action OnSprintEnd;
    public event Action OnJump;
    public event Action OnFalling;
    public event Action OnGround;


    public float Speed { get { return _speed; } private set { } }
    public float SprintSpeed { get { return _sprintSpeed; } private set { } }
    public bool IsAiming { get { return _isAiming; } private set { } }

    public bool _canMove = true;

    [SerializeField] float _speed, _sprintSpeed, _rotationSpeed, _gravity, _jumpImpulse, _jumpCoolDown, _aimAngleOffset, _aimMoveSpeed;
    [SerializeField] CharacterController _cc;
    [SerializeField] InputProvider _inputProvider;
    [SerializeField] NetworkAnimatorController _netAnimatorController;
    [SerializeField] SphereGroundChecker _groundChecker;


    private float _gravityVelocity, _jumpCDTimer;
    private bool _inJump, _aimRotationOffsetReached, _isAiming, _isSprinting;
    

    private bool _previousIsGround, _previousIsAiming, _previousIsSprinting = false;
    private InputData _previousInputData;

    #region CallEvents
    [Command(requiresAuthority = false)]
    void CmdEventInvoke(EventCode e)
    {
        RpcEventInvoke(e);
    }

    [ClientRpc(includeOwner = false)]
    void RpcEventInvoke(EventCode e)
    {
        EventInvoke(e);
    }

    void EventInvoke(EventCode e)
    {
        switch (e)
        {
            case EventCode.OnMoveBegin:
                OnMoveBegin?.Invoke();
                break;
            case EventCode.OnMoveEnd:
                OnMoveEnd?.Invoke();
                break;
            case EventCode.OnSprintBegin:
                OnSprintBegin?.Invoke();
                break;
            case EventCode.OnSprintEnd:
                OnSprintEnd?.Invoke();
                break;
            case EventCode.OnFalling:
                OnFalling?.Invoke();
                break;
            case EventCode.OnJump:
                OnJump?.Invoke();
                break;
            case EventCode.OnGround:
                OnGround?.Invoke();
                break;
        }
    }

    void GlobalEventInvoke(EventCode e)
    {
        EventInvoke(e);
        CmdEventInvoke(e);
    }
    #endregion

    
    void CameraRelativeMove(Vector2 dir)
    {
        dir.Normalize();
        Vector3 relativeDir = (Camera.main.transform.forward * dir.y) + (Camera.main.transform.right * dir.x);
        relativeDir.y = 0;
        relativeDir.Normalize();

        if (relativeDir != Vector3.zero)
        {
            Rotate(relativeDir);
            _netAnimatorController.Sprint(_inputProvider.Data.Sprint);
        }
        else
        {
            _netAnimatorController.Sprint(false);
        }
        float speedModify = 0;
        if (_inputProvider.Data.Sprint)
        {
            _isSprinting = true;
            speedModify = _sprintSpeed;
        }
        else
        {
            _isSprinting = false;
            speedModify = _speed;
        }

        _cc.Move(relativeDir * speedModify * Time.fixedDeltaTime);
        _netAnimatorController.Move(0, (Mathf.Abs(relativeDir.x) > 0 || Mathf.Abs(relativeDir.z) > 0) ? 1 : 0);

    }
    void AimingMove(Vector2 dir)
    {
        Vector3 relativeDir = (Camera.main.transform.forward * dir.normalized.y) + (Camera.main.transform.right * dir.normalized.x);
        relativeDir.y = 0;
        relativeDir.Normalize();
        _cc.Move(relativeDir * _aimMoveSpeed * Time.fixedDeltaTime);
        _netAnimatorController.Move(dir.x, dir.y);
    }
    void Rotate(Vector3 dir)
    {
        Vector3 lookRotation = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), _rotationSpeed);
    }
    void CheckAimAngleOffset(Vector3 dir)
    {
        Vector3 lookRotation = new Vector3(dir.x, 0, dir.z);
        Vector3 playerLook = transform.forward;
        playerLook.y = 0;
        float angle = Vector3.Angle(playerLook, lookRotation);
        if (angle > _aimAngleOffset)
        {
            _aimRotationOffsetReached = true;
        }
        if (angle < _aimAngleOffset / 10)
        {
            _aimRotationOffsetReached = false;
        }

    }
    void Jump()
    {
        if (_groundChecker.IsGrounded && !_groundChecker.IsSloping && _jumpCDTimer <= 0)
        {
            DisableGroundChecker();
            Invoke(nameof(EnableGroundChecker), Time.fixedDeltaTime);
            _inJump = true;
            GlobalEventInvoke(EventCode.OnJump);
            GlobalEventInvoke(EventCode.OnMoveEnd);
            _gravityVelocity = -_jumpImpulse;
            _netAnimatorController.SetTrigger("Jump");


            _jumpCDTimer = _jumpCoolDown;
        }
    }
    void Ground()
    {
        GlobalEventInvoke(EventCode.OnGround);
        _inJump = false;
        if (_inputProvider.Data.RawMoveVector != Vector2.zero)
        {
            GlobalEventInvoke(EventCode.OnMoveBegin);
        }
    }
    void AffectGravity()
    {
        if (_previousIsGround && !_groundChecker.IsGrounded && !_inJump)
        {
            GlobalEventInvoke(EventCode.OnFalling);

            _gravityVelocity = 0;
        }
        if (!_groundChecker.IsGrounded)
        {
            _gravityVelocity = Mathf.Clamp(_gravityVelocity += _gravity * Time.fixedDeltaTime, -100, 200);
        }
        else
        {
            _gravityVelocity = _gravity;
        }

        _cc.Move(Vector3.down * _gravityVelocity * Time.fixedDeltaTime);
        _netAnimatorController.Falling(!_groundChecker.IsGrounded);
    }
    void DisableGroundChecker()
    {
        _groundChecker.Disable();
    }
    void EnableGroundChecker()
    {
        _groundChecker.Enable();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            AffectGravity();
            if(_canMove)
            {
                Vector2 moveDir = new Vector2(_inputProvider.Data.RawMoveVector.x, _inputProvider.Data.RawMoveVector.y);


                if (moveDir != Vector2.zero)
                {
                    if (_inputProvider.WMInputProvider.Aim)
                    {
                        _isSprinting = false;
                        if (moveDir != Vector2.zero)
                        {
                            Rotate(Camera.main.transform.forward);
                            AimingMove(moveDir);
                            _isAiming = true;
                        }
                    }
                    else
                    {
                        CameraRelativeMove(moveDir);
                    }
                }
                else
                {
                    if (_inputProvider.WMInputProvider.Aim)
                    {
                        CheckAimAngleOffset(Camera.main.transform.forward);
                        if (_aimRotationOffsetReached)
                        {
                            Rotate(Camera.main.transform.forward);
                            _isAiming = true;
                        }
                    }
                    _netAnimatorController.Move(0, 0);
                }
                // aim or noaim movement

                if (_inputProvider.Data.Jump)
                {
                    Jump();
                } //jump on press space

                if (_previousInputData.RawMoveVector == Vector2.zero && _inputProvider.Data.RawMoveVector != Vector2.zero && _groundChecker.IsGrounded)
                {
                    GlobalEventInvoke(EventCode.OnMoveBegin);

                }
                else if (_previousInputData.RawMoveVector != Vector2.zero && _inputProvider.Data.RawMoveVector == Vector2.zero)
                {
                    GlobalEventInvoke(EventCode.OnMoveEnd);

                }



                if (_previousIsSprinting == false && _isSprinting)
                {
                    GlobalEventInvoke(EventCode.OnSprintBegin);
                }
                else if (_previousIsSprinting == true && !_isSprinting)
                {
                    GlobalEventInvoke(EventCode.OnSprintEnd);
                }

                if (_previousIsGround == false && _groundChecker.IsGrounded)
                {
                    Ground();
                }

                if (_previousIsAiming == false && _isAiming)
                {
                    GlobalEventInvoke(EventCode.OnSprintEnd);

                }

                if (_groundChecker.IsGrounded)
                {
                    if (_jumpCDTimer > 0)
                        _jumpCDTimer -= Time.fixedDeltaTime;
                }

                _previousIsSprinting = _isSprinting;
                _previousIsAiming = _isAiming;
                _previousIsGround = _groundChecker.IsGrounded;
                _previousInputData = _inputProvider.Data;
            }
        }
    }
}

enum EventCode
{
    OnMoveBegin,
    OnMoveEnd,
    OnSprintBegin,
    OnSprintEnd,
    OnJump,
    OnFalling,
    OnGround
}
