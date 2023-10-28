using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitDecalsDrawer : MonoBehaviour
{
    [SerializeField] private DecalProjector _decal;
    [SerializeField] private float _lifeTime = 5;
    private IShootable _shootable;

    private RaycastHit hit;

    private void Awake()
    {
        _shootable = GetComponent<IShootable>();
    }
    private void OnEnable()
    {
        _shootable.OnHit += SpawnDecal;
    }

    private void OnDisable()
    {
        _shootable.OnHit -= SpawnDecal;
    }

    void SpawnDecal(Vector3 shootSource, RaycastHit hit)
    {
        this.hit = hit;
        Vector3 reflectDir = Vector3.Reflect(hit.point - shootSource, hit.normal);
        DecalProjector dec = Instantiate(_decal, hit.point, Quaternion.LookRotation(reflectDir));
        dec.transform.SetParent(hit.transform);
        Destroy(dec.gameObject, _lifeTime);
    }
}
