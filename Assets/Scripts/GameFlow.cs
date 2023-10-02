using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameFlow : MonoBehaviour
{
 

    public GameObject[] BlurCard_GroupOne;
    public GameObject[] BlurCard_GroupTwo;
    public GameObject[] BlurCard_GroupThree;
    public GameObject[] BlurCard_GroupFour;
    public GameObject[] BlurCard_GroupFive;
    public GameObject[] BlurCard_Room3_TrashCan;
    public GameObject[] BlurCard_Room3_Receipt;
    public GameObject[] BlurCard_Room3_HDD;

    public GameObject Key;
    public GameObject HDD;
    public GameObject Cable;
    public GameObject TrashCan;
    public GameObject NewCableFromReciept;
    public GameObject NewBinFromRoom3;
    public GameObject EmptyPrefab;
    public PlayerInteract PlayerInteract;
    public GameObject QuestionMark_Prefab;
    private GameObject _questionMark;
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


        ToggleBlurCard(BlurCard_GroupFive, true);
        SetBlurCardWeight(BlurCard_GroupFive, 0.2f);
        SetBlurCardTint(BlurCard_GroupFive, Color.white);


        ToggleBlurCard(BlurCard_Room3_TrashCan, true);
        SetBlurCardWeight(BlurCard_Room3_TrashCan, 0.2f);
        SetBlurCardTint(BlurCard_Room3_TrashCan, Color.white);


        ToggleBlurCard(BlurCard_Room3_Receipt, true);
        SetBlurCardWeight(BlurCard_Room3_Receipt, 0.2f);
        SetBlurCardTint(BlurCard_Room3_Receipt, Color.white);


        ToggleBlurCard(BlurCard_Room3_HDD, true);
        SetBlurCardWeight(BlurCard_Room3_HDD, 0.2f);
        SetBlurCardTint(BlurCard_Room3_HDD, Color.white);

        Key.SetActive(false);
        HDD.SetActive(false);
        Cable.SetActive(false);
        TrashCan.SetActive(false);
        NewBinFromRoom3.SetActive(false);
        NewCableFromReciept.SetActive(false);

        _questionMark = Instantiate(QuestionMark_Prefab);
        _questionMark.SetActive(false);


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
        StartCoroutine(GameEvent(SetTimer, Fade_Room3_BlurCard, DisableRoom3_BlurCard, Two_Seconds_ExitCondition));
    }


    void Fade_Room3_BlurCard() 
    {
        SetBlurCardWeight(BlurCard_GroupOne, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }
   
    void DisableRoom3_BlurCard() 
    {
        ToggleBlurCard(BlurCard_GroupOne, false);

        SetBlurCardTint(BlurCard_Room3_TrashCan, BlurCardTint);

        SetBlurCardTint(BlurCard_Room3_Receipt, BlurCardTint);

        SetBlurCardTint(BlurCard_Room3_HDD, BlurCardTint);

        Act_Two_ReleaseBeer_Room2();
    }
    void Act_Two_ReleaseBeer_Room2() 
    {
        StartCoroutine(GameEvent(null, null, Disable_Room2_BlurCard, Act_Two_RealseBeer_ExitCondition));
    }

    void Disable_Room2_BlurCard() 
    {
        StartCoroutine(GameEvent(SetTimer, Fade_Room2_BlurCard, DisableRoom2_BlurCard, Two_Seconds_ExitCondition));
    }

    void Fade_Room2_BlurCard() 
    {
        SetBlurCardWeight(BlurCard_GroupTwo, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }

    void DisableRoom2_BlurCard ()
    {
        ToggleBlurCard(BlurCard_GroupTwo, false);
        Cable.SetActive(true);
        HDD.SetActive(true);
        TrashCan.SetActive(true);

        Act_Three_InteractWithRoom2();
    }

    bool Act_Two_RealseBeer_ExitCondition() 
    {


        return PlayerInteract._previousHoldingObject.name == "Beer"
            && RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[1] 
           && PlayerInteract._currentHoldingObject == null
           && PlayerInteract._previousHoldingObject.InThisRoom == RoomSwitch._StaticRooms[1];
           
    }

    void Act_Three_InteractWithRoom2()
    {
        StartCoroutine(GameEvent(null, null, Start_Disable_Room3_Cable, Cable_Enter_Room3_ExitCondition));
        //StartCoroutine(GameEvent(null, null, Start_Disable_HDD, HDD_Enter_Room3_ExitCondition));
        StartCoroutine(GameEvent(null, null, Start_Disable_Room3_Bin, Bin_Enter_Room3_ExitCondition));
        StartCoroutine(GameEvent(null, null, Start_Unvail_Room1, Bin_Enter_Room1_ExitCondition));
    }

    #region Room3 Cable Interaction
    void Start_Disable_Room3_Cable()
    {
        PlayerInteract.CanReleaseObject = false;
        StartCoroutine(GameEvent(SetTimer, null, Delay_Disable_Room3_Cable, One_Seconds_ExitCondition));
    }
    void Delay_Disable_Room3_Cable()
    {
        
        PlayerInteract._currentHoldingObject.ActivateFunction();
        PlayerInteract._currentHoldingObject.gameObject.SetActive(false);
        StartCoroutine(GameEvent(SetTimer, Room3_Cable_Unvail_Animation, Disable_Room3_Cable_BlurCard, Two_Seconds_ExitCondition));
    }

    void Room3_Cable_Unvail_Animation()
    {
        SetBlurCardWeight(BlurCard_Room3_Receipt, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }

    void Disable_Room3_Cable_BlurCard()
    {
        PlayerInteract.CanReleaseObject = true;
        ToggleBlurCard(BlurCard_Room3_Receipt, false);
        NewCableFromReciept.SetActive(true);
        NewCableFromReciept.GetComponent<MemoryObj>().PlayEffect(Color.cyan);
    }

    bool Cable_Enter_Room3_ExitCondition()
    {
        if (!PlayerInteract._currentHoldingObject)
            return false;
        return PlayerInteract._currentHoldingObject.name == "Cable" && RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[2];

    }
    #endregion

    #region Room3 HDD Interaction
    void Start_Disable_HDD()
    {
        PlayerInteract.CanReleaseObject = false;
        StartCoroutine(GameEvent(SetTimer, null, Delay_Disable_HDD, One_Seconds_ExitCondition));
    }

    void Delay_Disable_HDD()
    {
        PlayerInteract._currentHoldingObject.ActivateFunction();
        PlayerInteract._currentHoldingObject.gameObject.SetActive(false);
        PlayerInteract.CanReleaseObject = true;

        StartCoroutine(GameEvent(SetTimer, null, Delay_Show_QuestionMark, One_Seconds_ExitCondition));
    }

    void Delay_Show_QuestionMark() 
    {
        _questionMark.SetActive(true);
        _questionMark.transform.position = PlayerInteract.transform.position + Vector3.up * 2f;
        StartCoroutine(GameEvent(SetTimer, null,DisableQuestionMark, Two_Seconds_ExitCondition));

    }
    void DisableQuestionMark() 
    {
        _questionMark.SetActive(false);
    }

    bool HDD_Enter_Room3_ExitCondition()
    {
        if (!PlayerInteract._currentHoldingObject)
            return false;
        return PlayerInteract._currentHoldingObject.name == "HDD" && RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[2];
    }

    #endregion

    #region Room3 Bin Interaction
    void Start_Disable_Room3_Bin()
    {
        PlayerInteract.CanReleaseObject = false;

        StartCoroutine(GameEvent(SetTimer, null, Delay_Disable_Bin, One_Seconds_ExitCondition));
    }

    void Delay_Disable_Bin()
    {
        PlayerInteract._currentHoldingObject.ActivateFunction();
        PlayerInteract._currentHoldingObject.gameObject.SetActive(false);
        PlayerInteract.CanReleaseObject = true;

        StartCoroutine(GameEvent(SetTimer, Room3_Bin_Unvail_Animation, Disable_Room3_Bin_BlurCard, Two_Seconds_ExitCondition));
    }

    void Room3_Bin_Unvail_Animation()
    {
        SetBlurCardWeight(BlurCard_Room3_TrashCan, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }

    void Disable_Room3_Bin_BlurCard()
    {
        ToggleBlurCard(BlurCard_Room3_TrashCan, false);
        NewBinFromRoom3.SetActive(true);
        NewBinFromRoom3.GetComponent<MemoryObj>().PlayEffect(Color.cyan);
    }

    bool Bin_Enter_Room3_ExitCondition()
    {
        if (!PlayerInteract._currentHoldingObject)
            return false;
        return PlayerInteract._currentHoldingObject.name == "TrashCan" && RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[2];
    }

    #endregion

    #region Room1 Bin Interaction
    void Start_Unvail_Room1()
    {
        StartCoroutine(GameEvent(SetTimer, Unvail_Room1, Disable_Room1_BlurCard, Two_Seconds_ExitCondition));
    }
    void Unvail_Room1()
    {
        SetBlurCardWeight(BlurCard_GroupFour, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
        SetBlurCardWeight(BlurCard_GroupThree, Mathf.Lerp(0.2f, 0f, (Time.time - _timer) / 2f));
    }
    void Disable_Room1_BlurCard()
    {
        ToggleBlurCard(BlurCard_GroupFour, false);
        ToggleBlurCard(BlurCard_GroupThree, false);
    }

    bool Bin_Enter_Room1_ExitCondition()
    {

        return (PlayerInteract._previousHoldingObject.name == "NewTrashCanFromRoom3" || PlayerInteract._previousHoldingObject.name == "TrashCan")
            && PlayerInteract._previousHoldingObject.InThisRoom == RoomSwitch._StaticRooms[0]
            && RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[0]
            && PlayerInteract._currentHoldingObject == null;


    }
    #endregion

    bool Two_Seconds_ExitCondition()
    {
        return Time.time - _timer > 2f;
    }

    bool One_Seconds_ExitCondition()
    {
        return Time.time - _timer > 1f;
    }

    bool Half_Seconds_ExitCondition()
    {
        return Time.time - _timer > 0.5f;
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
