using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    
    public CinemachineFreeLook FreeLookCamera { get; private set; }
    public CinemachineVirtualCamera AimCamera { get ; private set; }

    public AimCameraController AimCamController { get; private set; }

    [SerializeField] private CinemachineFreeLook _freeLookCameraPrefab;
    [SerializeField] private AimCameraController _aimCameraPrefab;

    
    public override void OnStartLocalPlayer()
    {
        FreeLookCamera = Instantiate(_freeLookCameraPrefab);
        FreeLookCamera.Follow = gameObject.transform;
        FreeLookCamera.LookAt = gameObject.transform;

        AimCameraController aimController = Instantiate(_aimCameraPrefab);
        aimController._controller = GetComponent<NetworkCharacterController>();
        AimCamera = aimController.GetVirtualCamera();

        AimCamController = aimController;
        SetHighestPriority(FreeLookCamera);
    }

    public override void OnStopLocalPlayer()
    {
        
        Destroy(AimCamController.gameObject);
        Destroy(FreeLookCamera.gameObject);
    }
    

    public void SetHighestPriority(CinemachineVirtualCameraBase cam)
    {
        CinemachineCore core = CinemachineCore.Instance;

        for (int i = 0; i < core.VirtualCameraCount; i++)
        {
            core.GetVirtualCamera(i).Priority = 0;
        }

        cam.Priority = 1;
    }

   

}
