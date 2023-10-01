
using UnityEngine;
using System;
using System.Net.NetworkInformation;
using System.Collections;
using Unity.VisualScripting;

public class RoomSwitch : MonoBehaviour
{

    public Room[] Rooms;

    public GameObject MainCam;
    public AnimationCurve SlidingAnimationCurve;
    public float CameraStartPosYOffset = 1f;
    public static Func<Vector3> OnStartSlide;
    public static Func<Vector3,Vector3,Vector3> OnSliding;
    public static Func<Vector3> OnEndSlide;

    private bool _hasEnteredSliding = false;
    private Bounds _roomBounds = new Bounds();

    private Vector2 _slidingInitialMousePos = Vector2.zero;

    private Vector2 _slidingTargetMousePos = Vector2.zero;
    private Vector3 _startCameraPos = Vector3.zero;
    

    private int _currentRoomIndex = 0;
    private Room _currentRoom;
    private Coroutine _slidingAnimation_Co;
    

    


    void Start()
    {
        Preparation();

    }

    void Preparation() 
    {
        _roomBounds = Utility.GetObjectBound(Rooms[0].gameObject);
        _startCameraPos = new Vector3(_roomBounds.center.x, _roomBounds.center.y + CameraStartPosYOffset, MainCam.transform.position.z);
        MainCam.transform.position = _startCameraPos;

        for (int i = 0; i < Rooms.Length; i++) 
        {
            Rooms[i].transform.position = Rooms[0].transform.position +
                new Vector3 (i * _roomBounds.extents.x * 2, 0, 0) ;
        }
    }

    void UpdateSlidingControl()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
          
            _slidingInitialMousePos = Input.mousePosition;

        }
        if (Input.GetMouseButton(1))
        {
            _slidingTargetMousePos = Input.mousePosition;
            Vector2 diff = _slidingTargetMousePos- _slidingInitialMousePos;
            float percentage = diff.x / Screen.width;

            if (Mathf.Abs( percentage) > 0.1f && !_hasEnteredSliding) 
            {
                _hasEnteredSliding = true;
                if (_slidingAnimation_Co != null)
                    StopCoroutine(_slidingAnimation_Co);
   
                _slidingAnimation_Co = StartCoroutine(RoomSlideAnimation(0.5f,percentage > 0));
            }
        }

        if (Input.GetMouseButtonUp(1))
            _hasEnteredSliding = false;

    }

    IEnumerator RoomSlideAnimation(float timeToComplete,bool toRight) 
    {
        float process = 0;
        Room activeRoom = GetRoomContainsPlayer();
        Vector3 slidingTargetCameraPos = Vector3.zero;
        Vector3 slidingInitialCamPos = MainCam.transform.position;
        Vector3 slidingInitialPlayerPos = OnStartSlide.Invoke();
        float boundOffset = (toRight ? -1 : 1) * _roomBounds.extents.x * 2;
        if (boundOffset + slidingInitialPlayerPos.x < Rooms[0].transform.position.x - _roomBounds.extents.x
               || boundOffset + slidingInitialPlayerPos.x > Rooms[0].transform.position.x + _roomBounds.extents.x * 2 * Rooms.Length - _roomBounds.extents.x)
            boundOffset = 0;
        if (activeRoom) 
        {
           float camOffset = 
                Mathf.Min(
                Mathf.Max(
                    activeRoom.transform.position.x + boundOffset,
                    _startCameraPos.x),
                    _startCameraPos.x + _roomBounds.extents.x * 2 * (Rooms.Length - 1));
            slidingTargetCameraPos = new Vector3(camOffset,slidingInitialCamPos.y,slidingInitialCamPos.z) ;
        }
        else
            Debug.LogWarning("No Rooms Detact Player");


        while (process < timeToComplete)
        {
            process += Time.deltaTime;

            float percentage = SlidingAnimationCurve.Evaluate(process / timeToComplete);
            Vector3 finalCamPos = Vector3.Lerp (slidingInitialCamPos,slidingTargetCameraPos,percentage) ;
            Vector3 finalPlayerPos = slidingInitialPlayerPos + boundOffset * Vector3.right;
            OnSliding?.Invoke( finalPlayerPos,slidingInitialPlayerPos);
            MainCam.transform.position = finalCamPos;


            yield return null;
        }
        _hasEnteredSliding = false;
        OnEndSlide?.Invoke();

    }

    Room GetRoomContainsPlayer() 
    {
        foreach (Room r in Rooms) 
        {
            if (r.player!= null)
                return r;
        }
        return null;
    }
    void Update()
    {
        UpdateSlidingControl();
    }
}
