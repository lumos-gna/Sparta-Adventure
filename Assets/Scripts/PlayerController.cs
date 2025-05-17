using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rigid;
    
    
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
    }

    private void OnDestroy()
    {
        moveInputChannel.OnEventRasied -= SetMoveInputDir;
    }
    

    void SetMoveInputDir(Vector2 inputDir) => _moveInputDelta = inputDir * moveSpeed;

    void Move()
    {
        Vector3 dir = transform.forward * _moveInputDelta.y + transform.right * _moveInputDelta.x;
        
        dir.y = rigid.velocity.y;

        rigid.velocity = dir;
    }

}
