using System;
using System.Collections;
using UnityEngine;

public class PlayerConditionHandler : MonoBehaviour
{
    [SerializeField] private float maxStamina;
    [SerializeField] private float regenStaminaRate;
    [SerializeField] private float regenStaminaDelay;
    [SerializeField] private float jumpStamina;
    
    [SerializeField] private FloatEventChannelSO changedStaminaChannel;
    
    [SerializeField] private VoidEventChannelSO jumpedChannel;

    
    private float _curStamina;
    
    private Coroutine _staminaRegenCoroutine;

    private void Awake()
    {
        StartRegenStamina();

        _curStamina = maxStamina;
    }

    private void Start()
    {
        jumpedChannel.OnEventRaised += UseStamina;
    }

    private void OnDestroy()
    {
        jumpedChannel.OnEventRaised -= UseStamina;
    }

    void UseStamina()
    {
        StartRegenStamina();
        
        _curStamina = Mathf.Max(0, _curStamina - jumpStamina);
        
        changedStaminaChannel.Raise(_curStamina / maxStamina);
    }

    void StartRegenStamina()
    {
        if (_staminaRegenCoroutine != null)
        {
            StopCoroutine(_staminaRegenCoroutine);
        }
        
        _staminaRegenCoroutine = StartCoroutine(StaminaRegenCoroutine());
    }

    IEnumerator StaminaRegenCoroutine()
    {
        yield return new WaitForSeconds(regenStaminaDelay);
        
        while(_curStamina < maxStamina)
        {
            _curStamina = Mathf.Min(maxStamina, _curStamina + regenStaminaRate * Time.deltaTime);
        
            changedStaminaChannel.Raise(_curStamina / maxStamina);
            
            yield return null;
        }
        
        _curStamina = maxStamina;
    }
}
