using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSound : MonoBehaviour
{
    [SerializeField] List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField] float _volume, _minDistance, _maxDistance, _maxDurationInSequence;
    [SerializeField] private bool _randomSequence;


    private int _pointer = 0;
    
    public void Play(bool IsLastShoot)
    {
        
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = _volume;
        audioSource.spatialBlend = 1;
        audioSource.minDistance = _minDistance;
        audioSource.maxDistance = _maxDistance;
        audioSource.Stop();

        if (_randomSequence && _clips.Count > 1)
            _pointer = Random.Range(0, _clips.Count);

        if(_pointer <= _clips.Count-1)
            audioSource.clip = _clips[_pointer];

        audioSource.Play();
        if(IsLastShoot)
            Destroy(audioSource, audioSource.clip.length);
        else
            Destroy(audioSource, _maxDurationInSequence);

        if (_pointer < _clips.Count)
            _pointer++;
        else
            _pointer = 0;
    }
}
