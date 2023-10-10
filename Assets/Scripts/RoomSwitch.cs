
using UnityEngine;
using System;
using System.Collections;

public class RoomSwitch : MonoBehaviour
{

    public Room[] Rooms;
    public static Room[] _StaticRooms;

    public AnimationCurve SlidingAnimationCurve;

    public static Func<Vector3> OnRequestPlayerPos;
    public static Action<Vector3> OnSliding;
    public static Action OnEndSlide;


    public static Func<Vector3> OnRequestCameraPos;
    public static Action<Vector3> OnSetCameraPos;

    private bool _hasEnteredSliding = false;
    private Bounds _roomBounds = new Bounds();

    private Vector2 _slidingInitialMousePos = Vector2.zero;

    private Vector2 _slidingTargetMousePos = Vector2.zero;
    private Vector3 _startCameraPos = Vector3.zero;
    
    private Room _currentRoom;
    private Coroutine _slidingAnimation_Co;

    void Start()
    {
        Preparation();
        SetStartRoom(3);
    }
    private void OnEnable()
    {
        _StaticRooms = Rooms;
    }
    private void OnDisable()
    {
        _StaticRooms = null;

    }
    void Preparation() 
    {
        
        _roomBounds = Utility.GetObjectBound(Rooms[0].gameObject);
        _startCameraPos = new Vector3(_roomBounds.center.x, OnRequestCameraPos().y, OnRequestCameraPos().z);
        OnSetCameraPos?.Invoke(_startCameraPos);

        for (int i = 0; i < Rooms.Length; i++) 
        {
            Rooms[i].transform.position = Rooms[0].transform.position +
                new Vector3 (i * _roomBounds.extents.x * 2, 0, 0) ;
        }
    }

    void SetStartRoom(int num) 
    {
        Vector3 initialCamerapos = OnRequestCameraPos();
        Vector3 initialPlayerPos = OnRequestPlayerPos.Invoke();
        Vector3 diff = Rooms[num - 1].transform.position - Rooms[0].transform.position;

        OnSliding.Invoke(initialPlayerPos + diff);
        OnSetCameraPos?.Invoke( initialCamerapos + diff);
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

            if (Mathf.Abs(percentage) > 0.1f && !_hasEnteredSliding)
            {

                if (_slidingAnimation_Co != null) 
                    StopCoroutine(_slidingAnimation_Co);
                _slidingAnimation_Co = StartCoroutine(RoomSlideAnimation(0.5f,percentage > 0));
                _hasEnteredSliding = true;
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
        Vector3 slidingInitialCamPos = OnRequestCameraPos();
        Vector3 slidingInitialPlayerPos = OnRequestPlayerPos();
        Vector3 diff = slidingInitialPlayerPos - slidingInitialCamPos;
        float boundOffset = (toRight ? -1 : 1) * _roomBounds.extents.x * 2;
        if (activeRoom) 
        {
           float camOffset = 
                Mathf.Min(
                Mathf.Max(
                    Utility.GetObjectBound(activeRoom.gameObject).center.x + boundOffset,
                    _startCameraPos.x),
                    _startCameraPos.x + _roomBounds.extents.x * 2 * (Rooms.Length - 1));;
            slidingTargetCameraPos = new Vector3(camOffset,slidingInitialCamPos.y,slidingInitialCamPos.z) ;
        }
        else
            Debug.LogWarning("No Rooms Detact Player");

    

        while (process < timeToComplete)
        {
            process += Time.deltaTime;
   
            float percentage = SlidingAnimationCurve.Evaluate(process / timeToComplete);
            Vector3 finalCamPos = Vector3.Lerp (slidingInitialCamPos,slidingTargetCameraPos,percentage) ;
            Vector3 finalPlayerPos = finalCamPos + diff;
            OnSetCameraPos?.Invoke(finalCamPos);
            OnSliding?.Invoke( finalPlayerPos);
            yield return null;

        }
        _hasEnteredSliding = false;
        OnEndSlide?.Invoke();

    }

    public static Room GetRoomContainsPlayer() 
    {
        foreach (Room r in _StaticRooms) 
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
