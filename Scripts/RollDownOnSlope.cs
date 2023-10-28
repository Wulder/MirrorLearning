using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RollDownOnSlope : NetworkBehaviour
{

    [SerializeField] private SphereGroundChecker _gChecker;
    [SerializeField] private CharacterController _cc;
    [SerializeField] private float _slopingSpeed;

    private bool _startSloping = false;
    private float _velocity;
    void Start()
    { 
        _cc = GetComponent<CharacterController>();
    }

    Vector3 _slope;
    void FixedUpdate()
    {
        if (!isLocalPlayer) Destroy(this);

        if(_gChecker.IsSloping)
        {
            Vector3 reflect = Vector3.Reflect(Vector3.down, _gChecker.GroundHit.normal);            
            _slope = Vector3.Cross(Vector3.Cross(-reflect, _gChecker.GroundHit.normal),_gChecker.GroundHit.normal);

            if(!_startSloping)
            {
                _velocity = 0;
                _startSloping = true;
            }

            _velocity += Time.fixedDeltaTime * _slopingSpeed * (9.8f * (Mathf.Abs(_gChecker.SlopeAngle)/90));
            _cc.Move(_slope * Time.fixedDeltaTime * _velocity);
        }
        else
        {
            _velocity = 0;
            _startSloping = false;
        }
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawLine(_gChecker.GroundHit.point, _gChecker.GroundHit.point + _gChecker.GroundHit.normal);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_gChecker.GroundHit.point, _gChecker.GroundHit.point+_slope);
    }
}
