using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform cameraContainer;

    
    [Space(10f)]
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    
    private Vector2 _moveInputDelta;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    
    private void Start()
    {
        moveInputChannel.OnEventRasied += SetMoveInputDir;
    }
    
    private void FixedUpdate()
    {
        Move();

        Rotation();
    }

    private void OnDestroy()
    {
        moveInputChannel.OnEventRasied -= SetMoveInputDir;
    }
    

    void SetMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir * moveSpeed;

    
    void Move()
    {
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 dir = camForward.normalized * _moveInputDelta.y + camRight.normalized * _moveInputDelta.x;
        
        dir.y = rigid.velocity.y;
        
        rigid.velocity = dir;
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
}
