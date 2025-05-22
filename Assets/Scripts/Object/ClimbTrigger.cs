using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ClimbTrigger : MonoBehaviour
{
    public Vector3 InteractInfoPos => transform.position;

    private PlayerController _player;

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerExit(Collider other)  
    {
        if (_player != null)
        {
            if (other.gameObject == _player.gameObject)
            {
                _player.ChangeState(IPlayerState.Type.Basic);
                _player = null;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player.transform.position.y < transform.position.y + _collider.bounds.extents.y)
            {
                _player = player;
                
                _player.transform.rotation = Quaternion.LookRotation(transform.forward * -1);
                
                _player.ChangeState(IPlayerState.Type.Climb);
            }
        }
    }
}
