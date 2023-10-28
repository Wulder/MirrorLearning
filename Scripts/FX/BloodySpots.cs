using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodySpots : MonoBehaviour
{

    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject _bloodySpotPrefab;

    [SerializeField] private float _spotLifeTime;
    [SerializeField] private int _maxSpots;
    private int _spots;

    

    private void OnParticleCollision(GameObject other)
    {

        if (_spots < _maxSpots)
        {
            int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
            GameObject spot = Instantiate(_bloodySpotPrefab, collisionEvents[0].intersection, Quaternion.LookRotation(-collisionEvents[0].normal));
            
            
            _spots++;
        }

    }
}
