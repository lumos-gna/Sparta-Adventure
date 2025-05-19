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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private VoidEventChannelSO jumpInputChannel;


    private bool _isJumping;
    
    private Vector2 _moveInputDelta;
    private Vector3 _moveDir;
    
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
        jumpInputChannel.OnEventRaised += Jump;
    }
    

    void UpdateMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir * moveSpeed;

    
    void Move()
    {
        Vector3 dirForward = cameraContainer.forward.normalized *  _moveInputDelta.y;
        Vector3 dirRight = cameraContainer.right.normalized *  _moveInputDelta.x;

        _moveDir = dirForward + dirRight;
        
        _moveDir.y = rigid.velocity.y;
        
        rigid.velocity = _moveDir;
    }


    void Jump()
    {
        if (_isJumping == false)
        {
            if (IsGrounded())
            {
                StartCoroutine(JumpCoroutine());
            }
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
    
        Vector3 bottomPos = transform.position - new Vector3(0, coll.bounds.extents.y, 0);
        
        Vector3 point1 = bottomPos + Vector3.up * height;
        Vector3 point2 = bottomPos - Vector3.up * height;

        return Physics.CheckCapsule(point1, point2,  coll.radius, groundLayer);
    }

    IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        
        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        _isJumping = false;
    }
}
