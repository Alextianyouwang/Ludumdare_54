using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameFlow : MonoBehaviour
{
    // 1. Player Hold Beer Bottle, Unlock Surrounding. 
    // 2.  When enter Room 2 while holding beer, unlock HDD, Trashcan, Package (Wrong Cable)

    public GameObject Room3_BlurCard;
    public PlayerInteract PlayerInteract;
    private float _timer;

    void Start()
    {
        Room3_BlurCard.SetActive(true);
        Room3_BlurCard.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_Weight", 0.2f);

        Act_One_GrabBeer();
    }


    void Act_One_GrabBeer() 
    {
        StartCoroutine(GameEvent(null, null, DisableRoom3_BlurCard_WaitSeconds, Act_One_GrabBeer_ExitCondition));
    }
    bool Act_One_GrabBeer_ExitCondition() 
    {
        if (PlayerInteract._currentObject)
            return PlayerInteract._currentObject.name.Equals("Beer") && PlayerInteract._currentObject._isBeingHold;
        else return false;
    }
    void DisableRoom3_BlurCard_WaitSeconds() 
    {
        StartCoroutine(GameEvent(SetTimer, Fade_Room3_BlurCard, DisableRoom3_BlurCard, DisableRoom3_BlurCard_WaitSeconds_ExitCondition));
    }


    void Fade_Room3_BlurCard() 
    {
        Room3_BlurCard.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_Weight", Mathf.Lerp(0.2f,0f, (Time.time - _timer) / 2f));
    }
    bool DisableRoom3_BlurCard_WaitSeconds_ExitCondition() 
    {
       return Time.time - _timer > 2f;
    }
    void DisableRoom3_BlurCard() 
    {
        Room3_BlurCard.SetActive(false);
    }

    void SetTimer() 
    {
        _timer = Time.time;
    }
    IEnumerator GameEvent(Action beforeStart,Action whileDoing, Action afterDone, Func<bool> exitCondition) 
    {
        beforeStart?.Invoke();
        while (!exitCondition.Invoke()) 
        {
            whileDoing?.Invoke();
            yield return null;
        }
        afterDone?.Invoke();
    }
    IEnumerator WaitSeconds(Action before, float time, Action after)
    {
        before.Invoke();
        yield return new WaitForSeconds(time);
        after.Invoke();
    }
}
