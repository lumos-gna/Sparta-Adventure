using UnityEngine;
using UnityEngine.Events;



public class GenericEventChannelSO<T> : ScriptableObject
{
    public event UnityAction<T> OnEventRasied;

    public void Raise(T data) => OnEventRasied?.Invoke(data);
}