using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FX Settings", menuName = "FX Settings")]
public class FXSettings : ScriptableObject
{
    public List<HitSurface> HitSurfaces { get { return _hitSurfaces; } private set { } }
    public int MaxBloodDecals { get { return _maxBloodDecals; } private set { } }

    [SerializeField] private List<HitSurface> _hitSurfaces = new List<HitSurface>();
    [SerializeField] private int _maxBloodDecals;

    
}
