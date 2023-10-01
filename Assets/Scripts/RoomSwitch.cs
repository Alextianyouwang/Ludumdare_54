
using UnityEngine;
using System;

public class RoomSwitch : MonoBehaviour
{

    public GameObject[] Rooms;

    public GameObject MainCam;
    public GameObject Player;
    public static Action<bool> OnStartSlide;
    public static Action<Vector3> OnSliding;
    public static Action<bool> OnEndSlide;

    private bool _isSlidingHolding = false;
    private Bounds _roomBounds = new Bounds();

    private Vector2 _slidingInitialMousePos = Vector2.zero;
    private Vector3 _slidingInitialCamPos = Vector3.zero;

    private Vector2 _slidingTargetMousePos = Vector2.zero;


    


    void Start()
    {
        Preparation();

    }

    void Preparation() 
    {
        _roomBounds = Utility.GetObjectBound(Rooms[0]);
        for (int i = 0; i < Rooms.Length; i++) 
        {
            Rooms[i].transform.position = Rooms[0].transform.position +
                new Vector3 (i * _roomBounds.extents.x * 2,0,0) ;
        }
    }

    void UpdateSlidingControl()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            _slidingInitialMousePos = Input.mousePosition;
            _slidingInitialCamPos = MainCam.transform.position;
            OnStartSlide?.Invoke(false);

        }
        if (Input.GetMouseButton(1))
        {
            _slidingTargetMousePos = Input.mousePosition;
            Vector2 diff = _slidingTargetMousePos- _slidingInitialMousePos;
            float percentage = diff.x / Screen.width;
            Vector3 offsetValue = Vector3.right * _roomBounds.extents.x * 2 * percentage;
            MainCam.transform.position = _slidingInitialCamPos  + offsetValue;
            OnSliding?.Invoke(offsetValue);
        }
        if (Input.GetMouseButtonUp(1)) 
        {
            OnEndSlide?.Invoke(true);
        }
    }
    void Update()
    {
        UpdateSlidingControl();
    }
}
