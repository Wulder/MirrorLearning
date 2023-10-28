using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DevTools : NetworkBehaviour
{
    [SerializeField] PlayerCamera _playerCamera;
    [SerializeField] GameObject _uiRoot;


    private bool _isActive = false;
    private CursorLockMode _cursorMode = CursorLockMode.Locked;

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
            Destroy(this);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _isActive = !_isActive;
            if (_isActive)
                Show();
            else
                Hide();
        }
    }

    void Show()
    {
        _playerCamera.FreeLookCamera.enabled = false;
        _playerCamera.AimCamController.enabled = false;
        _uiRoot.SetActive(true);
        _cursorMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Hide()
    {
        _playerCamera.FreeLookCamera.enabled = true;
        _playerCamera.AimCamController.enabled = true;
        _uiRoot?.SetActive(false);
        Cursor.lockState = _cursorMode;
    }


    #region SpawnNpc
    private Vector3 _spawnNpcPosition = Vector3.zero;
    public void SpawnNpc(string npc)
    {
        if (isServer)
            GameManager.Instance.Spawn(npc, _spawnNpcPosition);
        else
            GameManager.Instance.RequestSpawn(npc,_spawnNpcPosition);

    }

   
    #endregion


}


