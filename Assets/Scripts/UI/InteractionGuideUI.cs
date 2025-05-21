using UnityEngine;

public class InteractionGuideUI : MonoBehaviour
{
   [Header("Events")]
   [SerializeField] private InteractableEventChannelSO toggleDetectInteractableChannel;


   private IInteractable _target;

   private Camera _camera;
   private RectTransform _rectTransform;
   private CanvasGroup _canvasGroup;
   
   private void Awake()
   {
      _camera = Camera.main;
      _rectTransform = GetComponent<RectTransform>();
      _canvasGroup = GetComponent<CanvasGroup>();
   }

   private void Start()
   {
      toggleDetectInteractableChannel.OnEventRaised += ToggleUI;

      Hide();
   }
   
   private void OnDestroy()
   {
      toggleDetectInteractableChannel.OnEventRaised -= ToggleUI;
   }
   
   private void Update()
   {
      if (_target != null)
      {
         _rectTransform.position = _camera.WorldToScreenPoint(_target.InfoPos);
      }
   }


   void ToggleUI(IInteractable interactable)
   {
      if (interactable != null)
      {
         Show(interactable);
      }
      else
      {
         Hide();
      }
   }
   
   void Hide()
   {
      _canvasGroup.alpha = 0;

      _target = null;
   }

   void Show(IInteractable interactable)
   {
      _canvasGroup.alpha = 1;

      _target = interactable;
      
      _rectTransform.position = _camera.WorldToScreenPoint(interactable.InfoPos);
   }
}
