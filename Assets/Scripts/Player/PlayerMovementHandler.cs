using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private CapsuleCollider coll;
    [SerializeField] private Transform cameraContainer;

    
    [Space(10f)]
    [Header("Setting")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float defaultMoveSpeed;
    
    [Space(5f)]
    [SerializeField] private float jumpForce;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private VoidEventChannelSO jumpInputChannel;
    [SerializeField] private VoidEventChannelSO onJumpChannel;


    private float _moveSpeed;
    
    private bool _isGrounded;
    private bool _isClimb;
    
    private Vector2 _moveInputDelta;
    private Vector3 _moveDir;

    private Coroutine _jumpChargingCoroutine;

    private void Awake()
    {
        _moveSpeed = defaultMoveSpeed;
    }

    private void Start()
    {
        moveInputChannel.OnEventRaised += UpdateMoveInputDir;
        jumpInputChannel.OnEventRaised += Jump;
    }
    
    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        
        Move();

        if (!_isClimb)
        {
            Rotation();
        }
    }

    private void OnDestroy()
    {
        moveInputChannel.OnEventRaised -= UpdateMoveInputDir;
        jumpInputChannel.OnEventRaised -= Jump;
    }

    public void SetClimb(bool isActive)
    {
        _isClimb = isActive;
        rigid.useGravity = !isActive;
        
        Vector3 vel = rigid.velocity;

        vel.y = 0;
            
        rigid.velocity = vel;
    }
    
    
    
    void UpdateMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir;

    
    void Move()
    {
        if (_isClimb)
        {
            Vector3 dirForward = transform.up *  _moveInputDelta.y;
            Vector3 dirRight = transform.right *  _moveInputDelta.x;
            
            _moveDir = (dirForward + dirRight) * _moveSpeed;
        }
        else
        {
            Vector3 dirForward = cameraContainer.forward *  _moveInputDelta.y;
            Vector3 dirRight = cameraContainer.right *  _moveInputDelta.x;
            
            _moveDir = (dirForward + dirRight) * _moveSpeed;
            
            _moveDir.y = rigid.velocity.y;
        }
    
        
        rigid.velocity = _moveDir;
    }

    void Jump()
    {
        if (_isGrounded)
        {
            Vector3 velocity = rigid.velocity;
            velocity.y = 0;
            rigid.velocity = velocity;
        
            rigid.AddForce(transform.up * jumpForce , ForceMode.Impulse);
            
            onJumpChannel.Raise();
        }
    }

    
    void Rotation()
    {
        if (_moveDir != Vector3.zero)
        {
            var playerRot = cameraContainer.eulerAngles;

            playerRot.x = 0;
            
            transform.eulerAngles = playerRot;
        }
    }
    
    bool IsGrounded()
    {
        float height = 0.001f;
    
        Vector3 bottomPos = transform.position - new Vector3(0, coll.bounds.extents.y, 0);

        Vector3 point1 = bottomPos + new Vector3(0, height, 0);
        Vector3 point2 = bottomPos - new Vector3(0, height, 0);

        return Physics.CheckCapsule(point1, point2,  coll.radius, groundLayer);
    }
}
