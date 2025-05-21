using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image healthBar;

    private PlayerConditionHandler _playerConditionHandler;

    private void Start()
    {
        _playerConditionHandler = FindAnyObjectByType<PlayerConditionHandler>();
    }

    private void Update()
    {
        if (_playerConditionHandler != null)
        {
            healthBar.fillAmount = _playerConditionHandler.CurHealth / _playerConditionHandler.MaxHealth;
            staminaBar.fillAmount = _playerConditionHandler.CurStamina / _playerConditionHandler.MaxStamina;
        }
    }
}
