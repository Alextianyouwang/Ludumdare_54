
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    private bool _allowMovement = true;
    public static Action<float> OnSharePlayerSpeed;
    public static Action<float> OnShareDistanceMoved;
    Vector3 _currentpos, _prevPos;
    private float _totalDist = 0;


    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        GetSpeed();
        if (_allowMovement)
        CheckInput();
    }

    void GetSpeed() 
    {
        _currentpos = transform.position;
        float diff = (_currentpos - _prevPos).magnitude;
        _totalDist += diff;
        OnSharePlayerSpeed?.Invoke(diff/Time.deltaTime);
        OnShareDistanceMoved?.Invoke(_totalDist);
        _prevPos = _currentpos;
       
    }
    private void OnEnable()
    {

        RoomSwitch.OnRequestPlayerPos += SharePosition;
        RoomSwitch.OnSliding += PassiveMove;
        RoomSwitch.OnEndSlide += ReturnBackToNormalMove;
    }
    private void OnDisable()
    {

        RoomSwitch.OnRequestPlayerPos -=  SharePosition;
        RoomSwitch.OnSliding -= PassiveMove;
        RoomSwitch.OnEndSlide -= ReturnBackToNormalMove;

    }

    Vector3 SharePosition() 
    {
        return transform.position;
    }
    void PassiveMove(Vector3 target) 
    {
        transform.position = target;

    }
    void ReturnBackToNormalMove() 
    {
        
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void CheckInput() 
    {
        _horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        _vertical = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
    }
    private void Movement()
    {
        Vector3 force = (_horizontal + _vertical).normalized * _moveSpeed;
        _rb.AddForce(force, ForceMode.Force);
    
    }
    
}
