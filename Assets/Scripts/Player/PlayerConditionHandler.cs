using System.Collections;
using UnityEngine;


public class PlayerConditionHandler : MonoBehaviour
{
    public float MaxHealth => maxHealth;
    public float CurHealth => _curHealth;
    public float MaxStamina => maxStamina;
    public float CurStamina => _curStamina;

    
    [SerializeField] private float maxHealth;
    [SerializeField] private float regenHealthRate;
    [SerializeField] private float regenHealthDelay;

    [Space(10f)]
    [SerializeField] private float maxStamina;
    [SerializeField] private float regenStaminaRate;
    [SerializeField] private float regenStaminaDelay;
    
    private float _curStamina;
    private float _curHealth;
    
    private Coroutine _staminaRegenCoroutine;
    private Coroutine _healthRegenCoroutine;

    private void Awake()
    {
        _curStamina = maxStamina;
        _curHealth = maxHealth;
    }


    public void ChangeStamina(float amount)
    {
        _curStamina += amount;

        _curStamina = Mathf.Clamp(_curStamina, 0, maxStamina);
        
        if (_staminaRegenCoroutine != null)
        {
            StopCoroutine(_staminaRegenCoroutine);
        }
        
        _staminaRegenCoroutine = StartCoroutine(StaminaRegenCoroutine());
    }

    public void ChangeHelath(float amount)
    {
        _curHealth += amount;

        _curHealth = Mathf.Clamp(_curHealth, 0, maxHealth);
        
        if (_healthRegenCoroutine != null)
        {
            StopCoroutine(_healthRegenCoroutine);
        }
        
        _healthRegenCoroutine = StartCoroutine(HealthRegenCoroutine());
    }


    IEnumerator StaminaRegenCoroutine()
    {
        yield return new WaitForSeconds(regenStaminaDelay);
        
        while(_curStamina < maxStamina)
        {
            _curStamina = Mathf.Min(maxStamina, _curStamina + regenStaminaRate * Time.deltaTime);
        
            yield return null;
        }
        
        _curStamina = maxStamina;
    }
    
    IEnumerator HealthRegenCoroutine()
    {
        yield return new WaitForSeconds(regenHealthDelay);
        
        while(_curHealth < maxHealth)
        {
            _curHealth = Mathf.Min(maxHealth, _curHealth + regenHealthDelay * Time.deltaTime);
        
            yield return null;
        }
        
        _curHealth = maxHealth;
    }
}
