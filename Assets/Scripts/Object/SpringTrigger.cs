using System;
using System.Collections;
using UnityEngine;

public class SpringTrigger : MonoBehaviour
{
    [SerializeField] private float maxPressTime;
    [SerializeField] private float maxForce;
    [SerializeField] private float maxPressDepth;


    private bool _isPressing;

    private float _curPressTime;

    private Vector3 _defaultPos;

    private Rigidbody _targetRigid;
    private Coroutine _pressCoroutine;

    private void Awake()
    {
        _defaultPos = transform.position;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rigid))
        {
            Vector3 dir = rigid.position - transform.position;

            if (dir.y > 0)
            {
                _targetRigid = rigid;

                if (!_isPressing)
                {
                    if (_pressCoroutine != null)
                    {
                        StopCoroutine(_pressCoroutine);
                    }
                    _pressCoroutine = StartCoroutine(PressCoroutine());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_pressCoroutine != null)
        {
            _isPressing = false;
            transform.position = _defaultPos;

            StopCoroutine(_pressCoroutine);
        }
    }

    IEnumerator PressCoroutine()
    {
        _isPressing = true;
        
        _curPressTime = 0;

        transform.position = _defaultPos;

        float pressRate = 0;
        
        while (_curPressTime < maxPressTime)
        {
            _curPressTime += Time.deltaTime;

            pressRate = _curPressTime / maxPressTime;

            float pressDepth = Mathf.Lerp(0, maxPressDepth, pressRate);

            Vector3 pressPos = _defaultPos;

            pressPos.y -= pressDepth;

            transform.position = pressPos;
            
            yield return null;
        }

        _targetRigid.AddForce(_targetRigid.transform.up * (maxForce * pressRate), ForceMode.Impulse);

        transform.position = _defaultPos;
        _curPressTime = 0;

        _isPressing = false;
    }
}
