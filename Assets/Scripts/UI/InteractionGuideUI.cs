using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractionGuideUI : MonoBehaviour
{
   [SerializeField] private CanvasGroup canvasGroup;

   [SerializeField] private RectTransform[] updateRectTransforms;

   [Space(10f)]
   [SerializeField] private TextMeshProUGUI messageInfoText;
   [SerializeField] private TextMeshProUGUI keyInfoText;

   
   [Space(10f)]
   [Header("Events")]
   [SerializeField] private InteractableEventChannelSO enterInteractableChannel;
   [SerializeField] private InteractableEventChannelSO exitInteractableChannel;

   private void Start()
   {
      enterInteractableChannel.OnEventRaised += ShowUI;
      exitInteractableChannel.OnEventRaised += HideUI;

      canvasGroup.alpha = 0;
   }

   private void OnDestroy()
   {
      enterInteractableChannel.OnEventRaised -= ShowUI;
      exitInteractableChannel.OnEventRaised -= HideUI;
   }

   void ShowUI(IInteractable interactable)
   {
      canvasGroup.alpha = 1;

      messageInfoText.text = interactable.DescriptionText;
      keyInfoText.text = interactable.KeyText;

      for (int i = 0; i < updateRectTransforms.Length; i++)
      {
         LayoutRebuilder.ForceRebuildLayoutImmediate(updateRectTransforms[i]);
      }
   }

   void HideUI(IInteractable interactable) =>  canvasGroup.alpha = 0;

}
