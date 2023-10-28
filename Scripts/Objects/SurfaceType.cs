using Unity.VisualScripting;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    
    public Surface Surface { get { return _surface; } private set { } }

    [SerializeField] private Surface _surface;


    
}


