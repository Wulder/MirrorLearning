using Mirror;
using Mirror.Examples.Benchmark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    #region FX Settings
    public FXSettings FxSettings { get { return _fxSettings; } private set { } }
    public FxManager FxManager { get { return _fxManager; } private set { } }
    [Header("Settings")]
    [SerializeField] FXSettings _fxSettings;
    [SerializeField] FxManager _fxManager;
    #endregion
    #region NPCSettings
    public NPCList NPCList => _npcList;
    [SerializeField] private NPCList _npcList;
    #endregion

    #region Players
    public PlayersManager PlayersManager { get { return _playersManager; } private set { } }
    [SerializeField] private PlayersManager _playersManager;
    #endregion

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        RegisterNpcPrefabs();
    }

    #region SpawnNpc
    [Server]
    public void Spawn(string npc, Vector3 pos)
    {
        GameObject prefab = _npcList.NPCs.Find(n => n.name == npc).gameObject;
        if(prefab)
        {
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
    }

   // [Command(requiresAuthority = false)]
    public void RequestSpawn(string npc, Vector3 pos)
    {
        Spawn(npc, pos);
    }
    #endregion

    void RegisterNpcPrefabs()
    {
        foreach(NPCObject n in _npcList.NPCs)
        {
            NetworkClient.RegisterPrefab(n.gameObject);
            
        }
    }
}
