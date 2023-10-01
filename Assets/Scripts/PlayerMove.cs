using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    private bool _allowMovement = true;

    private Vector3 _recordedPosition = Vector3.zero;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (_allowMovement)
        CheckInput();
    }
    private void OnEnable()
    {
        RoomSwitch.OnStartSlide += PreparePasiveMovement;
        RoomSwitch.OnSliding += PassiveMovement;
        RoomSwitch.OnEndSlide += ToggelAllowMovement;
    }
    private void OnDisable()
    {
        RoomSwitch.OnStartSlide -= PreparePasiveMovement;
        RoomSwitch.OnSliding -= PassiveMovement;
        RoomSwitch.OnEndSlide -= ToggelAllowMovement;

    }

    private void PreparePasiveMovement(bool value) 
    {
        ToggelAllowMovement(value);
        _recordedPosition = transform.position;
    }
    private void PassiveMovement(Vector3 offsetPos) 
    {
        _rb.isKinematic = true;
        transform.position = _recordedPosition + offsetPos;

    }
    private void ToggelAllowMovement(bool value) 
    {
        _allowMovement = value;
    }
    private void FixedUpdate()
    {
        if (_allowMovement)
            Movement();

    }
    private void CheckInput() 
    {
        _horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        
    }
    private void Movement()
    {
        _rb.isKinematic = false;
        Vector3 force = (_horizontal + _vertical).normalized * _moveSpeed;
        _rb.AddForce(force, ForceMode.Force);
    }
    
}
