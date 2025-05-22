using UnityEngine;

public interface IInteractable
{
    public Vector3 InteractInfoPos { get; }
    public void Interact(PlayerController player);
}