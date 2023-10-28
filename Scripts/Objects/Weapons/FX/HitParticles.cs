using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class HitParticles : MonoBehaviour
{
   
    private IShootable _shootable;

    private RaycastHit hit;

    private void Awake()
    {
        _shootable = GetComponent<IShootable>();
    }
    private void OnEnable()
    {
        _shootable.OnHit += SpawnSparks;
    }

    private void OnDisable()
    {
        _shootable.OnHit -= SpawnSparks;
    }

    void SpawnSparks(Vector3 shootSource, RaycastHit hit)
    {
        this.hit = hit;
        Vector3 reflectDir = Vector3.Reflect(hit.point - shootSource, hit.normal);

        Surface surface = hit.collider.gameObject.GetSurfaceType();
       

        if (surface != Surface.None)
        {
            
            if(GameManager.Instance.FxSettings.HitSurfaces.Exists(p => p.Surface == surface))
            {
                ParticleSystem ps = Instantiate(GameManager.Instance.FxSettings.HitSurfaces.Find(p => p.Surface == surface).Particles, hit.point + (reflectDir.normalized * 0.03f), Quaternion.LookRotation(reflectDir));
                Destroy(ps.gameObject, 2);
            }
            
        }


    }

    
}
[Serializable]
public struct HitSurface
{
    public ParticleSystem Particles;
    public Surface Surface;
}

