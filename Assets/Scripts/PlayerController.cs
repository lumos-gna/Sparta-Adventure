using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    [SerializeField] private float maxJumpChargeTime;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private VoidEventChannelSO jumpStartInputChannel;
    [SerializeField] private VoidEventChannelSO jumpEndInputChannel;


    private float _moveSpeed;
    private float _jumpChargeRate;
    
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
        jumpStartInputChannel.OnEventRaised += JumpCharging;
        jumpEndInputChannel.OnEventRaised += Jump;
    }
    
    private void FixedUpdate()
    {
        Move();

        Rotation();
    }

    private void OnDestroy()
    {
        moveInputChannel.OnEventRaised -= UpdateMoveInputDir;
        jumpStartInputChannel.OnEventRaised -= JumpCharging;
        jumpEndInputChannel.OnEventRaised -= Jump;
    }
    

    void UpdateMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir;

    
    void Move()
    {
        Vector3 dirForward = cameraContainer.forward *  _moveInputDelta.y;
        Vector3 dirRight = cameraContainer.right *  _moveInputDelta.x;

        _moveDir = dirForward + dirRight;

        _moveDir.y = 0;
        
        _moveDir.Normalize();
        
        if (_isJumping == false)
        {
            rigid.MovePosition(rigid.position + _moveDir * _moveSpeed);
        }
    }

    void Jump()
    {
        if (_isJumping == false)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    void JumpCharging()
    {
        if (_jumpChargingCoroutine != null)
        {
            StopCoroutine(_jumpChargingCoroutine);
        }

        _jumpChargingCoroutine = StartCoroutine(JumpChargingCoroutine());
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

    IEnumerator JumpChargingCoroutine()
    {
        float chargeTime = 0;

        while (chargeTime < maxJumpChargeTime)
        {
            chargeTime += Time.deltaTime;
            
            _jumpChargeRate = chargeTime / maxJumpChargeTime;

            yield return null;
        }
    }

    IEnumerator JumpCoroutine()
    {
        _isJumping = true;

        _moveSpeed = defaultMoveSpeed * airMoveSpeedRate;
        
        rigid.AddForce((transform.up + _moveDir) * (jumpForce * _jumpChargeRate), ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(IsGrounded);
        
        _moveSpeed = defaultMoveSpeed;

        _isJumping = false;
    }
}
