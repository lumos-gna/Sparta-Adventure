using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractTrigger : MonoBehaviour, IInteractable
{
    public event UnityAction<PlayerController> OnInteract;
    public event Func<Vector3> GetInfoPosFunc;

    public Vector3 GetInfoPos() => GetInfoPosFunc();
    public void Interact(PlayerController player) => OnInteract?.Invoke(player);

}
