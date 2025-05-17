using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private Vector2EventChannelSO lookInputChannel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            moveInputChannel.Raise(context.ReadValue<Vector2>());
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveInputChannel.Raise(Vector2.zero);
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) => lookInputChannel.Raise(context.ReadValue<Vector2>());
}