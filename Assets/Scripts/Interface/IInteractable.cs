using UnityEngine;

public interface IInteractable
{
    public bool IsInteractable { get;}
    public Vector3 InteractInfoPos { get; }
    public void Interact(GameObject source);
}