using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public bool CanCreateBloodSpot { get { return _bloodSpots.Count < GameManager.Instance.FxSettings.MaxBloodDecals; } private set { } }

    private List<BloodSpot> _bloodSpots = new List<BloodSpot>();

    public void AddBloodSpot(BloodSpot bs)
    {
        if(CanCreateBloodSpot)
        {
            _bloodSpots.Add(bs);
        }
        else
        {
            RemoveBloodSpot(_bloodSpots.First());
            _bloodSpots.Add(bs);
        }
    }

    public void RemoveBloodSpot(BloodSpot bs)
    {
        _bloodSpots.Remove(bs);
        Destroy(bs.gameObject);
    }
}
