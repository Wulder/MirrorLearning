using UnityEngine;
public interface IAiming
{
    public void BeginAim();
    public void Aim(Vector3 dir);
    public void AimEnd();

}