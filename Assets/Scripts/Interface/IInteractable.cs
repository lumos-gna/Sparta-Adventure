using UnityEngine;

public interface IInteractable
{
    public Vector3 InfoPos { get; }
    public void Interact(GameObject source);
}