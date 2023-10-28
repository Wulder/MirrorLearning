using UnityEngine;
using UnityEngine.Events;

public interface IShootable
{
    public event UnityAction<Vector3,RaycastHit> OnHit;
    public event UnityAction<Vector3, Vector3> OnShoot;
    public float FireRate { get; set; }
    public bool IsAutomatic { get; set; }
    public Transform ShootSource { get; set; }

    public void BeginShooting();
    public void Shoot();
    public void ShootingEnd();
}
