using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCrosshair : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshairTransform;
    

    
    public void Show()
    {
        _crosshairTransform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _crosshairTransform.gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        _crosshairTransform.position = Camera.main.WorldToScreenPoint(pos);
    }
    private void Update()
    {
       
        
    }
}
