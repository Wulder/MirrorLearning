using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] ParticleSystem _ps;
    [SerializeField] Weapon _weapon;
    private IShootable _shootable;

    private void OnDisable()
    {
        _shootable.OnShoot -= Shoot;
    }
    void OnEnable()
    {
        _shootable = _weapon.GetComponent<IShootable>();
        _shootable.OnShoot += Shoot;
    }

    void Shoot(Vector3 src, Vector3 target)
    {
        _ps.transform.forward = target - src;
        _ps.Play();
    }
}
