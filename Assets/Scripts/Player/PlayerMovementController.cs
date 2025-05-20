using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
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
    [SerializeField] private float airMoveSpeedRate;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private VoidEventChannelSO jumpInputChannel;


    private float _moveSpeed;
    
    private bool _isJumping;
    
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
        Move();

        Rotation();
    }

    private void OnDestroy()
    {
        moveInputChannel.OnEventRaised -= UpdateMoveInputDir;
        jumpInputChannel.OnEventRaised -= Jump;
    }
    
    void UpdateMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir;

    
    void Move()
    {
        Vector3 dirForward = cameraContainer.forward *  _moveInputDelta.y;
        Vector3 dirRight = cameraContainer.right *  _moveInputDelta.x;

        _moveDir = dirForward + dirRight;

        _moveDir.y = 0;
        
        _moveDir.Normalize();
        
        rigid.MovePosition(rigid.position + _moveDir * _moveSpeed);
    }

    void Jump()
    {
        if (_isJumping == false)
        {
            StartCoroutine(JumpCoroutine());
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


    IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        
        Vector3 velocity = rigid.velocity;
        velocity.y = 0;
        rigid.velocity = velocity;
        
        rigid.AddForce((transform.up + _moveDir) * jumpForce , ForceMode.Impulse);
        
        _moveSpeed = defaultMoveSpeed * airMoveSpeedRate;
        
        yield return new WaitForSeconds(0.33f);
        
        yield return new WaitUntil(() => IsGrounded());
        
        _moveSpeed = defaultMoveSpeed;

        _isJumping = false;
    }
}
