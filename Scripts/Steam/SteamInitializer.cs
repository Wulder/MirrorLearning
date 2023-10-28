using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeSteamworks();
    }

    void InitializeSteamworks()
    {
        if (SteamManager.Initialized)
            Debug.Log(Steamworks.SteamUser.GetSteamID().m_SteamID);
        else
            SteamAPI.Init();
    }
}
