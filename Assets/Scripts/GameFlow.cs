using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameFlow : MonoBehaviour
{
    // 1. Player Hold Beer Bottle, Unlock Surrounding. 
    // 2.  When enter Room 2 while holding beer, unlock HDD, Trashcan, Package (Wrong Cable)

    public GameObject[] BlurCard_GroupOne;
    public GameObject[] BlurCard_GroupTwo;
    public GameObject[] BlurCard_GroupThree;
    public GameObject[] BlurCard_GroupFour;
    public GameObject[] BlurCard_Room3_TrashCan;
    public GameObject[] BlurCard_Room3_Receipt;
    public GameObject[] BlurCard_Room3_HDD;
    public PlayerInteract PlayerInteract;
    public Color BlurCardTint;
    private float _timer;

    void Start()
    {
        ToggleBlurCard(BlurCard_GroupOne, true);
        SetBlurCardWeight(BlurCard_GroupOne, 0.2f);
        SetBlurCardTint(BlurCard_GroupOne, Color.white);

        ToggleBlurCard(BlurCard_GroupTwo, true);
        SetBlurCardWeight(BlurCard_GroupTwo, 0.2f);
        SetBlurCardTint(BlurCard_GroupTwo, Color.white);


        ToggleBlurCard(BlurCard_GroupThree, true);
        SetBlurCardWeight(BlurCard_GroupThree, 0.2f);
        SetBlurCardTint(BlurCard_GroupThree, Color.white);



        ToggleBlurCard(BlurCard_GroupFour, true);
        SetBlurCardWeight(BlurCard_GroupFour, 0.2f);
        SetBlurCardTint(BlurCard_GroupFour, Color.white);


        ToggleBlurCard(BlurCard_Room3_TrashCan, true);
        SetBlurCardWeight(BlurCard_Room3_TrashCan, 0.2f);
        SetBlurCardTint(BlurCard_Room3_TrashCan, Color.white);


        ToggleBlurCard(BlurCard_Room3_Receipt, true);
        SetBlurCardWeight(BlurCard_Room3_Receipt, 0.2f);
        SetBlurCardTint(BlurCard_Room3_Receipt, Color.white);


        ToggleBlurCard(BlurCard_Room3_HDD, true);
        SetBlurCardWeight(BlurCard_Room3_HDD, 0.2f);
        SetBlurCardTint(BlurCard_Room3_HDD, Color.white);




        Act_One_GrabBeer();
    }

    void ToggleBlurCard(GameObject[] group, bool on) 
    {
        foreach (GameObject g in group)
            g.SetActive(on);
    }
    void SetBlurCardWeight(GameObject[] group, float target) 
    {
        foreach (GameObject g in group) 
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetFloat("_Weight", target);
            g.GetComponent<Renderer>().SetPropertyBlock(mpb);
        }
    }

    void SetBlurCardTint(GameObject[] group, Color col)
    {
        foreach (GameObject g in group)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetColor("_Color", col);
            g.GetComponent<Renderer>().SetPropertyBlock(mpb);
        }
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
        SetBlurCardWeight(BlurCard_GroupOne, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }
    bool DisableRoom3_BlurCard_WaitSeconds_ExitCondition() 
    {
       return Time.time - _timer > 2f;
    }
    void DisableRoom3_BlurCard() 
    {
        ToggleBlurCard(BlurCard_GroupOne, false);

        SetBlurCardTint(BlurCard_Room3_TrashCan, BlurCardTint);

        SetBlurCardTint(BlurCard_Room3_Receipt, BlurCardTint);

        SetBlurCardTint(BlurCard_Room3_HDD, BlurCardTint);
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

}
