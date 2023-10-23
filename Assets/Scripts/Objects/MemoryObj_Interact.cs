using System;
using System.Collections;
using UnityEngine;

public class MemoryObj_Interact : MonoBehaviour
{
    public bool _isBeingHold { get; private set; } = false;
    public bool _canBeRelease { get; private set; } = false;

    public void GotHold(Transform target)
    {

        if (_isBeingHold)
            return;
        if (!_isBeingHold)
            _isBeingHold = true;

         ActivateFunction();
        StartCoroutine(SnapToPos(target, 0.2f, StartContinueSnapToPos));
    }

    IEnumerator SnapToPos(Transform target, float time, Action<Transform> next)
    {
        float progress = 0;
        Vector3 startPos = transform.position;

        while (progress < time)
        {
            progress += Time.deltaTime;
            float percent = progress / time;
            transform.position = Vector3.Lerp(startPos, target.position, percent);
            yield return null;
        }
        next?.Invoke(target);
    }
    void StartContinueSnapToPos(Transform target)
    {
        StartCoroutine(ContinueSnapToPos(target, ContinueSnappingExitCondition, ResetCanbeRelease));
    }
    IEnumerator ContinueSnapToPos(Transform target, Func<bool> exitCondition, Action next)
    {
        while (!exitCondition.Invoke())
        {
            transform.position = target.position;
            yield return null;
        }
        next?.Invoke();
    }

    void ResetCanbeRelease()
    {
        _canBeRelease = false;
        _isBeingHold = false;
        ReleaseFunction();
    }

    public void GotReleased()
    {
        _canBeRelease = true;
    }

    bool ContinueSnappingExitCondition()
    {
        return _canBeRelease;
    }

    void ReleaseFunction()
    {
        GetComponent<MemoryObj_Effect>()?.PlayEffect(Color.red);
    }
    void ActivateFunction()
    {
        GetComponent<MemoryObj_Effect>()?.PlayEffect(Color.blue);
    }

}
