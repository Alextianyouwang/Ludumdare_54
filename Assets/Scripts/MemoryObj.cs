
using UnityEngine;
using System;
using System.Collections;

public class MemoryObj : MonoBehaviour
{
    public bool HaveActivateFunction = true;
    public bool HaveReleaseFunction = true;
    public bool _hasBeenActivated { get; private set; } = false;
    public bool _isBeingHold { get;private set; } = false;
    public bool _canBeRelease { get;private set; } = false;



    public Canvas TestCanvas;
    public GameObject ActivateEffect;

    public static Action<MemoryObj> OnInteract;

    public float DistToPlayer { get; set; } = 0f;

    private void Awake()
    {
        Highlight(false);
    }

    public void Activate() 
    {

        if (_hasBeenActivated)
            return;
        if (!_hasBeenActivated)
            _hasBeenActivated = true;

        ActivateFunction();

     
    }
    public void ActivateFunction() 
    {
        PlayEffect(Color.white);
    }

    public void PlayEffect(Color color) 
    {
        GameObject effect = Instantiate(ActivateEffect);
        ParticleSystem fx = effect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = fx.main;
        effect.transform.position = transform.position;
        main.startColor = color;
         fx.Play();
    }

    public void GotHold(Transform target) 
    {

        if (_isBeingHold)
            return;
        if (!_isBeingHold)
            _isBeingHold = true;
        StartCoroutine(SnapToPos(target, 0.2f, StartContinueSnapToPos));
    }

    IEnumerator SnapToPos(Transform target , float time, Action<Transform> next) 
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
        StartCoroutine(ContinueSnapToPos(target, ContinueSnappingExitCondition,ResetCanbeRelease));
    }
    IEnumerator ContinueSnapToPos(Transform target,Func<bool> exitCondition,Action next) 
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

    void ReleaseFunction() 
    {
        PlayEffect(Color.yellow);
    }

    public void GotReleased() 
    {
        _canBeRelease = true;
    }

    bool ContinueSnappingExitCondition() 
    {
        return _canBeRelease;
    }
    public void Highlight(bool value) 
    {
        TestCanvas.enabled = value;
    }

    
    private void OnTriggerEnter(Collider other)
    {

    }
    private void Update()
    {
        
    }
}
