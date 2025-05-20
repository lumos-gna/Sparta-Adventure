using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;

    [Space(10f)] [SerializeField] private FloatEventChannelSO changedStaminaChannel;

    private void Start()
    {
        changedStaminaChannel.OnEventRaised += UpdateStaminaBar;
    }

    private void OnDestroy()
    {
        changedStaminaChannel.OnEventRaised -= UpdateStaminaBar;
    }

    void UpdateStaminaBar(float ratio) => staminaBar.fillAmount = ratio;
}
