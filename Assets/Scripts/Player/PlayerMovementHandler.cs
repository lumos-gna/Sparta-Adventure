using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform cameraContainer;

    [Space(10f)]
    [Header("Setting")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float defaultMoveSpeed;
    
    [Space(5f)]
    [SerializeField] private float jumpForce;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private BoolEventChannelSO toggleClimbChannel;
    [SerializeField] private VoidEventChannelSO jumpedChannel;


    private float _moveSpeed;
    
    private bool _isGrounded;
    private bool _isClimb;
    
    private Rigidbody _rigid;
    private CapsuleCollider _coll;
    
    private Vector2 _moveInputDelta;

    private Coroutine _jumpChargingCoroutine;

    private Func<Vector3> _moveDirFunc;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _coll = GetComponent<CapsuleCollider>();
        
        _moveSpeed = defaultMoveSpeed;

        _moveDirFunc = SetBasicMove;
    }

    private void Start()
    {
        toggleClimbChannel.OnEventRaised += SetClimb;
    }
    
    private void OnDestroy()
    {
        toggleClimbChannel.OnEventRaised -= SetClimb;
    }
    
    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        
        _rigid.velocity = _moveDirFunc();

        if (!_isClimb)
        {
            Rotation();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed : 
                _moveInputDelta = context.ReadValue<Vector2>(); break;
            case InputActionPhase.Canceled :
                _moveInputDelta = Vector2.zero; break;
        }
    }
    
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }
  

    void SetClimb(bool isActive)
    {
        _isClimb = isActive;
        _rigid.useGravity = !isActive;
        
        Vector3 vel = _rigid.velocity;

        vel.y = 0;
            
        _rigid.velocity = vel;

        _moveDirFunc = isActive ? SetClimbMove : SetBasicMove;
    }
    
    Vector3 SetClimbMove()
    {
        Vector3 dirForward = transform.up *  _moveInputDelta.y;
        Vector3 dirRight = transform.right *  _moveInputDelta.x;
            
        return (dirForward + dirRight) * _moveSpeed;
    }

    Vector3 SetBasicMove()
    {
        Vector3 dirForward = cameraContainer.forward *  _moveInputDelta.y;
        Vector3 dirRight = cameraContainer.right *  _moveInputDelta.x;
        
        Vector3 moveDir = (dirForward + dirRight) * _moveSpeed;
            
        moveDir.y = _rigid.velocity.y;

        return moveDir;
    }
    

    void Jump()
    {
        if (_isGrounded)
        {
            Vector3 velocity = _rigid.velocity;
            velocity.y = 0;
            _rigid.velocity = velocity;
        
            _rigid.AddForce(transform.up * jumpForce , ForceMode.Impulse);
            
            jumpedChannel.Raise();
        }
    }
    
    void Rotation()
    {
        if (_moveInputDelta != Vector2.zero)
        {
            var playerRot = cameraContainer.eulerAngles;

            playerRot.x = 0;
            
            transform.eulerAngles = playerRot;
        }
    }
    
    bool IsGrounded()
    {
        float height = 0.001f;
        
        Vector3 bottomPos = _isClimb ? 
            transform.position - new Vector3(0,0,_coll.bounds.extents.x) :
            transform.position - new Vector3(0, _coll.bounds.extents.y, 0);

        Vector3 point1 = bottomPos + new Vector3(0, height, 0);
        Vector3 point2 = bottomPos - new Vector3(0, height, 0);

        return Physics.CheckCapsule(point1, point2,  _coll.radius, groundLayer);
    }
}
