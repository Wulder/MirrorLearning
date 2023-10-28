using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public List<GameObject> PlayerObjects { get { return _playersObjects; } private set { } }
    private List<GameObject> _playersObjects = new List<GameObject>();
    public void AddPlayer(NetworkPlayer player)
    {
        _playersObjects.Add(player.gameObject);
    }

    public void RemovePpayer(NetworkPlayer player)
    {
        _playersObjects.Remove(player.gameObject);
    }
        
    
}
