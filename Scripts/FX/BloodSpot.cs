using UnityEngine;

public class BloodSpot : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.FxManager.AddBloodSpot(this);
    }
    
}
