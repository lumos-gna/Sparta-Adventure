using System;
using UnityEngine;

public class InteractionGuideUI : MonoBehaviour
{
   private RectTransform _rectTransform;
   private CanvasGroup _canvasGroup;
   
   private Camera _camera;
   private PlayerInteractHandler _interactHandler;
   
   private void Awake()
   {
      _rectTransform = GetComponent<RectTransform>();
      _canvasGroup = GetComponent<CanvasGroup>();
      
      _canvasGroup.alpha = 0;
   }

   private void Start()
   {
      _camera = Camera.main;

      _interactHandler = FindAnyObjectByType<PlayerInteractHandler>();
   }
   
   private void LateUpdate()
   {
      if (_interactHandler.CurTarget != null)
      {
         _rectTransform.position = _camera.WorldToScreenPoint(_interactHandler.CurTarget.GetInfoPos());
         
         _canvasGroup.alpha = 1;
      }
      else
      {
         _canvasGroup.alpha = 0;
      }
   }
}
