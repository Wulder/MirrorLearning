using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    public bool IsEnabled { get { return _isEnabled; } private set { } }

   [SerializeField] private List<Rigidbody> _rbodies = new List<Rigidbody>();
   [SerializeField] private bool _isEnabled;
   [SerializeField] private Animator _animator;


    [ContextMenu("Enable")]
    public void Enable()
    {
        foreach(Rigidbody rb in _rbodies)
        {
            rb.isKinematic = false;
        }
        _animator.enabled = false;
        _isEnabled = true;
    }

    [ContextMenu("Disable")]
    public void Disable()
    {
        foreach (Rigidbody rb in _rbodies)
        {
            rb.isKinematic = true;
        }
        _animator.enabled = true;
        _isEnabled = false;
    }

    [ContextMenu("Load Rigidbodies")]
    void GetRigidbodies()
    {
        _rbodies = GetComponentsInChildren<Rigidbody>().ToList();
    }
}
