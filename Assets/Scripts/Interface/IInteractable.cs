using UnityEngine;

public interface IInteractable
{
    public Vector3 GetInfoPos();
    public void Interact(PlayerController player);
}