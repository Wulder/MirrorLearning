using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{

    public InputData Data { get; private set; }
    public WeaponManagerInputProvider WMInputProvider { get { return _wmInputProvider; } private set { } }

    [SerializeField] private WeaponManagerInputProvider _wmInputProvider;

    private Vector2 _moveVector;
    private Vector2 _rawMoveVector;
    private bool _sprint;
    private bool _jump; 
    
    private NetworkPlayer _player;
   
    
    void Start()
    {
        _player = GetComponent<NetworkPlayer>();
    }

    void Update()
    {

        if (_player.isLocalPlayer)
        {
            ReadMotion();
            ReadJump();
            SetData();
        }
        
    }

    void ReadMotion()
    {
        _moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rawMoveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _sprint = Input.GetKey(KeyCode.LeftShift);
    }

    void ReadJump()
    {
        _jump = Input.GetKey(KeyCode.Space);
    }

    void SetData()
    {
        Data = new InputData(_moveVector,_rawMoveVector,_sprint,_jump);
    }

   
}

public struct InputData
{
    public InputData(Vector2 moveVector, Vector2 rawMoveVector, bool sprint, bool jump)
    {
        MoveVector = moveVector;
        RawMoveVector = rawMoveVector;
        Sprint = sprint;
        Jump = jump;
    }
    public Vector2 MoveVector { get; private set; }
    public Vector2 RawMoveVector { get; private set; }
    public bool Sprint { get; private set; }
    public bool Jump { get; private set; }
}
