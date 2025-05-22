using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
  
    [SerializeField] private Transform cameraContainer;

    [Space(10f)]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float airMoveSpeedRate;
    
    [Space(5f)]
    [SerializeField] private float jumpForce;
    
    
    [Space(10f)]
    [SerializeField] private BoolEventChannelSO toggleClimbChannel;
    [SerializeField] private VoidEventChannelSO jumpedChannel;

    
    private float _moveSpeed;
    
    private Rigidbody _rigid;
    private CapsuleCollider _collider;
    private Coroutine _jumpChargingCoroutine;

    private PlayerController _controller;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _collider =  GetComponent<CapsuleCollider>();
        _controller = GetComponent<PlayerController>();
        
        _moveSpeed = defaultMoveSpeed;
    }

    private void Start()
    {
        toggleClimbChannel.OnEventRaised += InitClimbState;
    }
    
    private void OnDestroy()
    {
        toggleClimbChannel.OnEventRaised -= InitClimbState;
    }
    
    
    public void InitClimbState(bool isActive)
    {
        _rigid.useGravity = !isActive;
        
        Vector3 vel = _rigid.velocity;

        vel.y = 0;
            
        _rigid.velocity = vel;
    }
    
    public void RotationToCamera()
    {
        if (_controller.MoveInputDelta != Vector2.zero)
        {
            var playerRot = cameraContainer.eulerAngles;

            playerRot.x = 0;
            
            transform.eulerAngles = playerRot;
        }
    }
    
    
    public void BasicMove()
    {
        Vector2 inputDelta = _controller.MoveInputDelta;
        
        Vector3 moveDir = (cameraContainer.forward * inputDelta.y) + (cameraContainer.right * inputDelta.x);
        
        Vector3 velocity = moveDir * _moveSpeed;

        velocity.y = _rigid.velocity.y;
        
        _rigid.velocity = velocity;
    }
    
    
    public void ClimbMove()
    {
        Vector2 inputDelta = _controller.MoveInputDelta;
        
        Vector3 moveDir = transform.up * inputDelta.y;
        
        Vector3 velocity =  moveDir * _moveSpeed;

        velocity.x = 0;
        velocity.z = 0;
        
        _rigid.velocity = velocity;
    }


    public void AirMove()
    {
        Vector2 inputDelta = _controller.MoveInputDelta;
        
        Vector3 moveDir = (cameraContainer.forward * inputDelta.y) + (cameraContainer.right * inputDelta.x);

        moveDir.y = 0;


        Vector3 curVelocity = _rigid.velocity;

        curVelocity.y = 0;
        
        float curSpeed = Vector3.Dot(curVelocity, moveDir);

        float airSpeed = _moveSpeed * airMoveSpeedRate;
        
        if (curSpeed < airSpeed)
        {
            _rigid.AddForce(moveDir * airSpeed, ForceMode.Impulse);
        }

    }
    

    public void Jump()
    {
        Vector2 inputDelta = _controller.MoveInputDelta;

        Vector3 jumpDir = new Vector3(inputDelta.x, 0, inputDelta.y) + Vector3.up * 4f;
        
        _rigid.AddForce(jumpDir.normalized * jumpForce , ForceMode.Impulse);
            
        jumpedChannel.Raise();
    }
  
    public bool IsGrounded()
    {
        Vector3 checkPos = transform.position - new Vector3(0, _collider.bounds.extents.y - 0.3f, 0);
     
        return Physics.CheckSphere(checkPos, _collider.radius * 0.9f, groundLayer);
    }
}
