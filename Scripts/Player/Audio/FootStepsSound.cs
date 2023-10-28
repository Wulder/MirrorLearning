using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    public bool IsPlaying;

    [SerializeField] private List<AudioClip> _steps = new List<AudioClip>();
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _stepDuration = 1, _defaultSpeed, _sprintSpeed;
    [SerializeField] NetworkCharacterController _controller;

    private float _timer;
    private int _clipsPointer;
    private float _speed = 1;

    private void OnEnable()
    {
        _controller.OnMoveBegin += Play;
        _controller.OnMoveEnd += Pause;
        _controller.OnSprintBegin += SetSprintSpeed;
        _controller.OnSprintEnd += SetDefaultSpeed;
        _controller.OnFalling += Pause;
    }

    private void OnDisable()
    {
        _controller.OnMoveBegin -= Play;
        _controller.OnMoveEnd -= Pause;
        _controller.OnSprintBegin -= SetSprintSpeed;
        _controller.OnSprintEnd -= SetDefaultSpeed;
        _controller.OnFalling -= Pause;
    }
    void Start()
    {
        _timer = 0;
        _clipsPointer = 0;
        IsPlaying = false;
        SetSpeed(_controller.Speed);
    }

    void Update()
    {
        if(IsPlaying)
        {
            _timer += (Time.deltaTime * _speed);
            if(_timer >= _stepDuration)
            {
                _timer = 0;
                _audioSource.clip = _steps[_clipsPointer];
                _audioSource.Play();
                _clipsPointer++;

                if (_clipsPointer > _steps.Count - 1)
                    _clipsPointer = 0;
            }
        }
    }

    void Pause()
    {
        IsPlaying = false;
        _audioSource.Stop();
        _timer = 0;
        
    }

    void Play()
    {
        
        IsPlaying = true;
        _audioSource.Play();
        _timer = 0;
    }

    void SetSprintSpeed()
    {
     
        SetSpeed(_sprintSpeed);
    }
    void SetDefaultSpeed()
    {
    
        SetSpeed(_defaultSpeed);
    }

    void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
