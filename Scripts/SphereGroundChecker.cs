using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGroundChecker : MonoBehaviour
{

    public bool IsGrounded { get; private set; }
    public bool IsSloping { get; private set; }
    public float SlopeAngle { get { return _slopeAngle; } private set { } }
    public float MaxAngle { get { return _maxAngle; } private set { } }

    public RaycastHit GroundHit { get { return _groundHit; } private set { } }

    [SerializeField] float _checkGrounderRadius, _maxDistance, _maxAngle;
    [SerializeField] GameObject _origin;

    private RaycastHit _groundHit;
    private float _slopeAngle;



    private void FixedUpdate()
    {
        IsGrounded = Physics.SphereCast(_origin.transform.position, _checkGrounderRadius, Vector3.down, out _groundHit, _maxDistance);

        if (IsGrounded)
        {
            _slopeAngle = Vector3.Angle(Vector3.up, _origin.transform.position + (Vector3.down * _groundHit.distance) - _groundHit.point);

            if (_slopeAngle > _maxAngle)
                IsSloping = true;
            else
                IsSloping = false;

            return;
        }
        IsSloping = false;

    }

    public void Disable()
    {
        IsGrounded = false;
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_origin.transform.position, _origin.transform.position + (Vector3.down * _groundHit.distance));

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_origin.transform.position + (Vector3.down * _groundHit.distance), _checkGrounderRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_origin.transform.position + (Vector3.down * _groundHit.distance), 0.05f);
        Gizmos.DrawLine(_origin.transform.position + (Vector3.down * _groundHit.distance), _groundHit.point);

        if (!IsSloping)
            Gizmos.color = Color.red;
        else
            Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(_groundHit.point, 0.05f);



    }
}
