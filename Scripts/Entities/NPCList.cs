using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCList", menuName = "NPC/List")]
public class NPCList : ScriptableObject
{
    public List<NPCObject> NPCs => _npcs;
    [SerializeField] private List<NPCObject> _npcs = new List<NPCObject>();
}

[Serializable]
public struct NPCObject
{
    public string name;
    public GameObject gameObject;
}
