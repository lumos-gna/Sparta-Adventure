using System;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
   [SerializeField] private float moveSpeed;

   [SerializeField] private InteractTrigger interactTrigger;

   [SerializeField] private Transform[] movePoints;

   private bool _isMoveForward = false;

   private int _curIndex = 0;

   private void Awake()
   {
      interactTrigger.OnInteract += (player) =>
      {
         _isMoveForward = !_isMoveForward;
         
         _curIndex += _isMoveForward ? 1 : -1;

         _curIndex = Mathf.Clamp(_curIndex, 0, movePoints.Length - 1);
      };
      
      interactTrigger.GetInfoPosFunc += () => interactTrigger.transform.position;

      transform.position = movePoints[_curIndex].position;
   }

   private void FixedUpdate()
   {
      Move();
   }

   void Move()
   {
      var targetPos = movePoints[_curIndex].position;
      
      transform.position =  Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

      if (_isMoveForward)
      {
         if (_curIndex < movePoints.Length - 1)
         {
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
               _curIndex++;
            }
         }
      }
      else
      {
         if (_curIndex > 0)
         {
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
               _curIndex--;
            }
         }
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out PlayerController player))
      {
         player.transform.SetParent(transform);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.TryGetComponent(out PlayerController player))
      {
         player.transform.SetParent(null);
      }
   }

}
