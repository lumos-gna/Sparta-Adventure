using UnityEngine;

public interface IInteractable
{
    public string KeyText { get; }
    public string DescriptionText { get; }
    public void Interact(GameObject source);
}