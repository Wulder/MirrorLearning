using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeExtensions 
{
    
}

public static class GameobjectSurfaceExtension
{
    public static Surface GetSurfaceType(this GameObject gm)
    {
        SurfaceType st = gm.GetComponentInParent<SurfaceType>();
        if (st != null)
        {
            return st.Surface;
        }

        return Surface.None;
    }
}

public static class Vector3DIstanceExtension
{
    public static float Get2dDistance(this Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(new Vector3(vec1.x, 0, vec1.z), new Vector3(vec2.x, 0, vec2.z));
    }
}
