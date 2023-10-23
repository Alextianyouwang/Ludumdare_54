
using UnityEngine;
using System;

public class Player_MovementController : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    private bool _allowMovement = true,_allowShareMovementStats = true;
    public static Action<float> OnSharePlayerSpeed;
    public static Action<float> OnShareTotalDistTravel;
    Vector3 _currentpos, _prevPos;
    private float _totalDist = 0;


    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(_allowShareMovementStats)
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
        OnShareTotalDistTravel?.Invoke(_totalDist);
        _prevPos = _currentpos;
       
    }
    private void OnEnable()
    {

        Room_Controller.OnRequestPlayerPos += SharePosition;
        Room_Controller.OnSliding += PassiveMove;
        Room_Controller.OnEndSlide += ReturnBackToNormalMove;
        Room_Controller.OnStartSlidePlayer += StartPassiveMove;
    }
    private void OnDisable()
    {

        Room_Controller.OnRequestPlayerPos -=  SharePosition;
        Room_Controller.OnSliding -= PassiveMove;
        Room_Controller.OnEndSlide -= ReturnBackToNormalMove;
        Room_Controller.OnStartSlidePlayer -= StartPassiveMove;


    }

    Vector3 SharePosition() 
    {
        return transform.position;
    }

    void StartPassiveMove() 
    {
     
        _allowShareMovementStats = false;

    }
    void PassiveMove(Vector3 target) 
    {
        transform.position = target;

    }
    void ReturnBackToNormalMove() 
    {
        _prevPos = transform.position;
        _allowShareMovementStats = true;

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
