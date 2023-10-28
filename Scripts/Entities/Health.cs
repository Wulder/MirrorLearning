using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public event UnityAction<int,int> OnChange;
    public event UnityAction OnDead;

    public int CurrentHealth => _currentHealth;
    public int StartHealth => _startHealth;

    public bool IsAlive => _isAlive;

    [SyncVar(hook = nameof(OnHealthChangeHook))] int _currentHealth;
    [SyncVar] bool _isAlive;

    [SerializeField] int _startHealth;

    public override void OnStartServer()
    {
        _currentHealth = _startHealth;
        _isAlive = true;
    }
    
    void OnHealthChangeHook(int oldVal, int newVal)
    {
        if (oldVal == newVal) return;

        OnChange?.Invoke(oldVal,newVal);

        if (CurrentHealth <= 0 && _isAlive)
        {
            OnDead?.Invoke();
        }

    }


    [Server]
    public void SetDamage(int damage)
    {
        if(_isAlive)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _isAlive = false;
                _currentHealth = 0;
            }
               
        }
       

    }

    [Command(requiresAuthority = false)]
    public void CmdSetHealth(int health)
    {
        _currentHealth = health;
        if(_currentHealth > 0)
        {
            _isAlive = true;
        }    
    }
}
