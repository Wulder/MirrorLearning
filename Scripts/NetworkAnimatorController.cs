using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimatorController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Mirror.NetworkAnimator _netAnimator;
    [SerializeField] float _interpolationVal;
    [SerializeField] NetworkCharacterController _controller;

    private Vector2 _previousMoveVal = Vector2.zero;

    private void OnEnable()
    {
        _controller.OnMoveEnd += EndSprint;
    }

    private void OnDisable()
    {
        _controller.OnMoveEnd -= EndSprint;
    }
   

    public void Move(float x, float y)
    {
        Vector2 interpolatedVal = Vector2.Lerp(_previousMoveVal, new Vector2(x, y), _interpolationVal);
        _animator.SetFloat("YMove", interpolatedVal.y);
        _animator.SetFloat("XMove", interpolatedVal.x);
        _previousMoveVal = interpolatedVal;
    }
    public void Sprint(bool value)
    {
        
        _animator.SetBool("Sprint", value);
    }
    public void Falling(bool value)
    {
        _animator.SetBool("Falling", value);
    }
    public void Aiming(bool value)
    {
        _animator.SetBool("Aiming", value);
    }
    public void Knockout()
    {
        _animator.SetTrigger("Knockout");
        _animator.SetTrigger("Knockout");
    }
    public void GetUp()
    {
        _netAnimator.SetTrigger("GetUp");
        _netAnimator.SetTrigger("GetUp");
    }
    public void SetTrigger(string triggerName)
    {
        _netAnimator.SetTrigger(triggerName);
    }
    public void SetLayerWeight(int index, float value)
    {
        _animator.SetLayerWeight(index, value);
    }
    public void SetLayerWeight(string name, float value)
    {
        int index = _animator.GetLayerIndex(name);
        _animator.SetLayerWeight(index, value);
    }

    void EndSprint()
    {
        Sprint(false);
    }
}
