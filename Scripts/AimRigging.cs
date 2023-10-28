using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimRigging : MonoBehaviour
{
    public MultiAimConstraint BodyAim { get { return _bodyAim; } set { } }
    public MultiAimConstraint RightWist { get { return _rightWist; } set { } }
    public TwoBoneIKConstraint LeftHand { get { return _leftHand; } set { } }
    public Transform AimTarget { get { return _aimTarget; } private set { } }
    public Transform LeftHandGrab { get { return _leftHandGrab; } private set { } }
    public float Weight { get { return _rig.weight; } private set { } }

    [SerializeField] Transform _aimTarget, _leftHandGrab, _leftElbowHint;
    [SerializeField] Rig _rig;
    [SerializeField] MultiAimConstraint _bodyAim;
    [SerializeField] MultiAimConstraint _rightWist;
    [SerializeField] TwoBoneIKConstraint _leftHand;
    [SerializeField] float _interpolation;

    [HideInInspector]
    private Transform _leftHandGrabFollow, _aimTargetFollow, _leftElbowHintFollow;
    [HideInInspector]
    private Vector3 _aimTargetStartPos, _leftHandGrabStartPos, _leftElbowHintStartPos;
    [HideInInspector]
    public float Interpolation;


    private void Start()
    {
        _aimTargetStartPos = _aimTarget.localPosition;
        _leftHandGrabStartPos = _leftHandGrab.localPosition;
        _leftElbowHintStartPos = _leftElbowHint.localPosition;
        Interpolation = _interpolation;
    }
    private void Update()
    {
        if (_leftHandGrabFollow)
        {
            InterpolationFollow(_leftHandGrab, _leftHandGrabFollow,1);
        }
        if (_aimTargetFollow)
        {
            InterpolationFollow(_aimTarget, _aimTargetFollow);
        }
        if (_leftElbowHintFollow)
        {
            InterpolationFollow(_leftElbowHint, _leftElbowHintFollow);
        }

    }

    void InterpolationFollow(Transform t1, Transform t2, float time)
    {
        t1.position = Vector3.Slerp(t1.position, t2.position, time);
        t1.rotation = Quaternion.Slerp(t1.rotation, t2.rotation, time);
    }
    void InterpolationFollow(Transform t1, Transform t2)
    {
        InterpolationFollow(t1, t2, Interpolation);
    }
    public void SetLeftHandGrab(Transform t)
    {
        _leftHandGrabFollow = t;
    }

    public void SetAimTarget(Transform t)
    {
        _aimTargetFollow = t;
    }

    public void SetLeftElbowHint(Transform t)
    {
        _leftElbowHintFollow = t;
    }

    public void SetRigWeight(float w)
    {
        _rig.weight = w;
    }

    public void ResetTargets()
    {
        _aimTarget.localPosition = _aimTargetStartPos;
        _leftElbowHint.localPosition = _leftElbowHintStartPos;
        _leftHandGrab.localPosition = _leftHandGrabStartPos;

        _aimTargetFollow = null;
        _leftHandGrabFollow = null;
        _leftElbowHintFollow = null;
    }

    public void ResetInterpolation()
    {
        Interpolation = _interpolation;
    }

    public void StartSmoothWeightUp(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothUpWeight(speed));
    }
    public void StartSmoothWeightDown(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothDownWeight(speed));
    }

    IEnumerator SmoothUpWeight(float speed)
    {
        while (Weight < 1)
        {
            SetRigWeight(Weight + (Time.deltaTime * speed));
            yield return null;
        }
    }

    IEnumerator SmoothDownWeight(float speed)
    {
        while (Weight > 0)
        {
            SetRigWeight(Weight - (Time.deltaTime * speed));
            yield return null;
        }
    }



}
